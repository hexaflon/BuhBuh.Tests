using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Group;
using TestTest.Models.Db;
using System.Linq;

namespace ProjektInzynierski.Tests.GroupTest
{
    public class AddMemberCommandTests
    {
        private DatabaseContext GetContext(string name)
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(name)
                .Options;
            var context = new DatabaseContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            return context;
        }


        [Fact]
        public void Execute_ShouldAddParticipant()
        {

            using var context = GetContext("AddMemberTestDB");


            var command = new AddMemberCommand(context, 1, 42);

            command.Execute();

            Assert.Single(context.Participant);
            Assert.Equal(42, context.Participant.First().StudentId);
        }

        [Fact]
        public void Execute_ShouldAssignNextId()
        {
            using var context = GetContext("AddMemberTestDB");


            context.Participant.Add(new Participant { Id = 5, GroupId = 1, StudentId = 1 });
            context.SaveChanges();

            var command = new AddMemberCommand(context, 1, 2);
            command.Execute();

            Assert.Equal(6, context.Participant.Last().Id);
        }

        [Fact]
        public void Execute_ShouldAddToCorrectGroup()
        {
            using var context = GetContext("AddMemberTestDB");


            var command = new AddMemberCommand(context, 100, 50);
            command.Execute();

            Assert.Equal(100, context.Participant.First().GroupId);
        }

        [Fact]
        public void Execute_ShouldIncreaseCount()
        {
            using var context = GetContext("AddMemberTestDB");


            var command = new AddMemberCommand(context, 1, 1);

            var initialCount = context.Participant.Count();
            command.Execute();

            Assert.Equal(initialCount + 1, context.Participant.Count());
        }

        [Fact]
        public void Execute_MultipleCalls_ShouldIncrementId()
        {
            using var context = GetContext("AddMemberTestDB");

            var cmd1 = new AddMemberCommand(context, 1, 1);
            var cmd2 = new AddMemberCommand(context, 1, 2);

            cmd1.Execute();
            cmd2.Execute();

            Assert.Equal(1, context.Participant.First().Id);
            Assert.Equal(2, context.Participant.Last().Id);
        }
    }
}