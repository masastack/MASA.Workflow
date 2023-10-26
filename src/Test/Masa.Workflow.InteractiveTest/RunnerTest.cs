using System.Dynamic;


namespace Masa.Workflow.InteractiveTest;

public class RunnerTest
{
    [Theory]
    [InlineData("return 2 * 3;")]
    public async void CSharpCodeTest(string code)
    {
        var _cSharpRunner = new CSharpRunner();
        var obj = await _cSharpRunner.RunAsync<int>(code);
        Assert.Equal(6, obj);
    }

    [Fact]
    public async void NugetTest()
    {
        var code = """
            var product = new
            {
                Name = "Apple",
                Expiry = new DateTime(2008, 12, 28),
                Sizes = new string[] { "Small" }
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            return json;
        """;
        var _cSharpRunner = new CSharpRunner(new List<IMetadataReferenceProvider> { new NuGetMetadataReferenceProvider() });
        var obj = await _cSharpRunner.RunAsync<string>(code, packages: new List<string> { "Newtonsoft.Json,13.0.3" });
        Assert.True(obj.Length > 0);
    }

    [Fact]
    public async void ParamterTest()
    {
        var code = """
            return X + Y;
        """;
        var _cSharpRunner = new CSharpRunner();
        var obj = await _cSharpRunner.RunAsync<int>(code, globals: new Globals { X = 1, Y = 2 });

        Assert.Equal(3, obj);
    }

    [Fact]
    public async void ExpandoObjectTest()
    {
        var msg = new Msg();
        msg.Payload.Name = "Masa";
        var code = """
            return Payload.Name;
            """;

        var _cSharpRunner = new CSharpRunner();
        var name = await _cSharpRunner.RunAsync<string>(code, globals: msg);

        Assert.Equal("Masa", name);
    }
}

public class Globals
{
    public int X;
    public int Y;
}

public class Msg
{
    public dynamic Payload { get; set; } = new ExpandoObject();

}