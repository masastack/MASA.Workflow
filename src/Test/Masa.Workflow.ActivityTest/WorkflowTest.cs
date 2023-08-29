namespace Masa.Workflow.ActivityTest;

public class WorkflowTest
{

    [Fact]
    public void Test()
    {
        Mock<WorkflowContext> mockContext = new();
        //mockContext
        //    .Setup(ctx => ctx.CallActivityAsync<InventoryResult>(nameof(ReserveInventoryActivity), It.IsAny<InventoryRequest>(), It.IsAny<WorkflowTaskOptions>()))
        //    .Returns(Task.FromResult(inventoryResult));

        //// Run the workflow directly
        //OrderResult result = await new OrderProcessingWorkflow().RunAsync(mockContext.Object, order);

        //mockContext.Verify(
        //        ctx => ctx.CallActivityAsync<InventoryResult>(nameof(ReserveInventoryActivity), expectedInventoryRequest, It.IsAny<WorkflowTaskOptions>()),
        //        Times.Once());
    }
}
