using Xunit;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Question;
using TestTest.Models.Db;
using System.Linq;
using System.Collections.Generic;

namespace ProjektInzynierski.Tests.QuestionTest
{
    public class QuestionFacadeTests
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
            var context = GetContext("Q");

            context.QuestionType.Add(new QuestionType { Id = 10, Name = "prawda/fałsz", Description = "wielo" });
            context.SaveChanges();

            var facade = new QuestionFacade(context);

            var q = facade.CreateQuestion("ABC", 1, 1, 10, true);

            Assert.Single(context.Question);
            Assert.Equal("ABC", q.Text);
        }

        [Fact]
        public void CreateQuestion_ShouldAssignNextId()
        {
            var context = GetContext("Q");


            context.Question.Add(new TestTest.Models.Db.Question { Id = 5, Text = "Old" });
            context.QuestionType.Add(new QuestionType { Id = 11, Name = "prawda", Description = "jedno" });
            context.SaveChanges();

            var facade = new QuestionFacade(context);

            var q = facade.CreateQuestion("New", 1, 1, 11, true);

            Assert.Equal(6, q.Id);
        }

        [Fact]
        public void AddTrueFalseAnswers_ShouldCreateTwoAnswers()
        {
            var context = GetContext("Q");

            context.QuestionType.Add(new QuestionType { Id = 12, Name = "prawda" , Description = "prawda/fałsz"});
            context.SaveChanges();

            var facade = new QuestionFacade(context);

            facade.CreateQuestion("Q", 1, 1, 12, true);

            Assert.Equal(2, context.Answer.Count());
        }

        [Fact]
        public void GetQuestionList_ShouldReturnFilteredByCategory()
        {
            var context = GetContext("Q");

            context.Question.Add(new TestTest.Models.Db.Question { Id = 1, Text = "A", CategoryId = 2 });
            context.Question.Add(new TestTest.Models.Db.Question { Id = 2, Text = "B", CategoryId = 3 });
            context.SaveChanges();

            var facade = new QuestionFacade(context);

            var list = facade.GetQuestionList(categoryId: 3);

            Assert.Single(list);
            Assert.Equal(3, list.First().CategoryId);
        }

        [Fact]
        public void GetQuestionList_ShouldReturnAllWithIncludes()
        {
            var context = GetContext("Q");

            var q = new TestTest.Models.Db.Question
            {
                Id = 1,
                Text = "T",
                Answers = new List<Answer> { new Answer { Id = 1, Text = "A" } }
            };

            context.Question.Add(q);
            context.SaveChanges();

            var facade = new QuestionFacade(context);

            var list = facade.GetQuestionList();

            Assert.Single(list);
            Assert.Single(list.First().Answers);
        }
    }
}
