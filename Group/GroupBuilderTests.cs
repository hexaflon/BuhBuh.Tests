using Xunit;
using ProjektInzynierski.Pages.Group;
using TestTest.Models.Db;
using System.Collections.Generic;

public class GroupBuilderTests
{
    [Fact]
    public void SetID_ShouldSetId()
    {
        var builder = new GroupBuilder();
        var group = builder.SetID(10).Build();

        Assert.Equal(10, group.Id);
    }

    [Fact]
    public void SetName_ShouldSetName()
    {
        var builder = new GroupBuilder();
        var group = builder.SetName("Test").Build();

        Assert.Equal("Test", group.Name);
    }

    [Fact]
    public void AddParticipant_ShouldAddParticipant()
    {
        var builder = new GroupBuilder();
        var participant = new Participant { Id = 1 };
        var group = builder.AddParticipant(participant).Build();

        Assert.Contains(participant, group.Participants);
    }

    [Fact]
    public void AddParticipantList_ShouldAddAllParticipants()
    {
        var builder = new GroupBuilder();
        var list = new List<Participant> { new Participant { Id = 1 }, new Participant { Id = 2 } };
        var group = builder.AddParticipantList(list).Build();

        Assert.Equal(2, group.Participants.Count);
    }

    [Fact]
    public void SetTeacherId_ShouldSetTeacherId()
    {
        var builder = new GroupBuilder();
        var group = builder.SetTeacherId(99).Build();

        Assert.Equal(99, group.TeacherId);
    }
}
