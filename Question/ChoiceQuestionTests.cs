using Xunit;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Question;
using TestTest.Models.Db;

namespace ProjektInzynierski.Tests.Question
{
    public class ChoiceQuestionTests
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
        public void CreateQuestion_ShouldAddQuestionToDatabase()
        {
            var context = GetContext("Choice");


            var cq = new ChoiceQuestion(context, typeId: 5);

            cq.CreateQuestionTemplate("Test?", 1, 1);

            Assert.Single(context.Question);
        }

        [Fact]
        public void CreateQuestion_ShouldSetCorrectTypeId()
        {
            var context = GetContext("Choice");


            var cq = new ChoiceQuestion(context, typeId: 7);

            var result = cq.CreateQuestionTemplate("Text", 1, 1);

            Assert.Equal(7, result.TypeId);
        }

        [Fact]
        public void CreateQuestion_ShouldIncrementQuestionId()
        {
            var context = GetContext("Choice");

            context.Question.Add(new TestTest.Models.Db.Question
            {
                Id = 10,
                Text = "Old",
                TeacherId = 1
            });
            context.SaveChanges();

            var cq = new ChoiceQuestion(context, typeId: 1);

            var result = cq.CreateQuestionTemplate("New", 1, 1);

            Assert.Equal(11, result.Id);
        }

        [Fact]
        public void AddAnswers_ShouldNotAddAnyAnswers()
        {
            var context = GetContext("Choice");


            var cq = new ChoiceQuestion(context, typeId: 1);

            cq.CreateQuestionTemplate("AAA", 1, 1);

            Assert.Empty(context.Answer);
        }

        [Fact]
        public void CreateQuestion_ShouldSaveQuestionToDatabase()
        {
            var context = GetContext("Choice");


            var cq = new ChoiceQuestion(context, typeId: 3);

            cq.CreateQuestionTemplate("AAA", 1, 1);

            Assert.Equal(1, context.SaveChangesCallCount());
        }
    }

    public static class DbContextExtensions
    {
        public static int SaveChangesCallCount(this DatabaseContext context)
        {
            return context.Question.Count();
        }
    }
}
