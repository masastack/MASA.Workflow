var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
    options.MaxReceiveMessageSize = 3 * 1024 * 1024;
    options.EnableDetailedErrors = !builder.Environment.IsProduction();
}).AddJsonTranscoding();

builder.Services
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = "";
        options.RequireHttpsMetadata = false;
        options.Audience = "";
    });

builder.Services
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddEndpointsApiExplorer()
    .AddGrpcSwagger()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer xxxxxxxxxxxxxxx\"",
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var filePath = Path.Combine(System.AppContext.BaseDirectory, xmlFilename);
        options.IncludeXmlComments(filePath);
        options.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
    })
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddDomainEventBus(dispatcherOptions =>
    {
        dispatcherOptions
            .UseIntegrationEventBus<IntegrationEventLogService>(options => options.UseDapr().UseEventLog<WorkflowDbContext>())
            .UseEventBus(eventBusBuilder =>
            {
                eventBusBuilder.UseMiddleware(typeof(ValidatorMiddleware<>));
            })
            .UseUoW<WorkflowDbContext>(dbOptions => dbOptions.UseSqlServer())
            .UseRepository<WorkflowDbContext>();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    #region MigrationDb
    using var context = app.Services.CreateScope().ServiceProvider.GetService<WorkflowDbContext>();
    {
        context!.Database.EnsureCreated();
    }
    #endregion
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Used for Dapr Pub/Sub.
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapGrpcService<WorkflowService>();

app.UseHttpsRedirection();

app.Run();