using Xunit;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Group;
using TestTest.Models.Db;
using System.Linq;

public class DeleteGroupCommandTests
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
    public void Execute_ShouldRemoveGroup()
    {

        using var context = GetContext("DeleteGroupDB");


        var group = new Group { Id = 1, Name = "TestGroup", Participants = new List<Participant>() };
        context.Group.Add(group);
        context.SaveChanges();

        var command = new DeleteGroupCommand(context, 1);
        command.Execute();

        Assert.Empty(context.Group);
    }

    [Fact]
    public void Execute_ShouldRemoveParticipants()
    {
        using var context = GetContext("DeleteGroupDB");


        var group = new Group { Id = 1, Name = "G", Participants = new List<Participant> { new Participant { Id = 1, StudentId = 1 } } };
        context.Group.Add(group);
        context.SaveChanges();

        var command = new DeleteGroupCommand(context, 1);
        command.Execute();

        Assert.Empty(context.Participant);
    }

    [Fact]
    public void Execute_NonExistingGroup_ShouldNotFail()
    {
        using var context = GetContext("DeleteGroupDB");


        var command = new DeleteGroupCommand(context, 999);

        var ex = Record.Exception(() => command.Execute());
        Assert.Null(ex);
    }

    [Fact]
    public void Execute_ShouldDecreaseCount()
    {
        using var context = GetContext("DeleteGroupDB");

        var group = new Group { Id = 1, Name = "G" };
        context.Group.Add(group);
        context.SaveChanges();

        var initialCount = context.Group.Count();
        var command = new DeleteGroupCommand(context, 1);
        command.Execute();

        Assert.Equal(initialCount - 1, context.Group.Count());
    }

    [Fact]
    public void Execute_ShouldOnlyRemoveSpecifiedGroup()
    {
        using var context = GetContext("DeleteGroupDB");

        context.Group.AddRange(
            new Group { Id = 1, Name = "G1" },
            new Group { Id = 2, Name = "G2" }
        );
        context.SaveChanges();

        var command = new DeleteGroupCommand(context, 1);
        command.Execute();

        Assert.Single(context.Group);
        Assert.Equal(2, context.Group.First().Id);
    }
}
