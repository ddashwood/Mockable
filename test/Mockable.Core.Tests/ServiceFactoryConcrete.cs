namespace Mockable.Core.Tests;

internal class ServiceFactoryConcrete : ServiceFactoryBase
{
    public Mock<IMockCreator> MockCreatorMock { get; private set; }

    public ServiceFactoryConcrete()
        :this(GenerateMockMockCreator(out var mockCreatorMock), mockCreatorMock)
    {
    }

    private ServiceFactoryConcrete(IMockCreator mockCreator, Mock<IMockCreator> mockCreatorMock) :
        base(mockCreator)
    {
        MockCreatorMock = mockCreatorMock;
    }

    private static IMockCreator GenerateMockMockCreator(out Mock<IMockCreator> mock)
    {
        mock = new Mock<IMockCreator>();
        return mock.Object;
}
}
