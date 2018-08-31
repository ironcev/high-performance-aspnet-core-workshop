using Xunit;
using GettingThingsDone.ApplicationCore.Services;
using Moq;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using AutoFixture.Xunit2;
using System.Threading.Tasks;

namespace GettingThingsDone.Tests
{
    public class ActionServiceTests
    {
        [Theory, AutoData]
        public async Task Test1(int id)
        {
            var actionRepository = new Mock<IAsyncRepository<Action>>();
            var listRepository = new Mock<IRepository<ActionList>>();
            var projectRepository = new Mock<IRepository<Project>>();
            var sut = new ActionService(actionRepository.Object, listRepository.Object, projectRepository.Object);

            var result = await sut.GetAction(id);

            Assert.Equal(ServiceCallStatus.EntityNotFound, result.Status);
        }
    }
}
