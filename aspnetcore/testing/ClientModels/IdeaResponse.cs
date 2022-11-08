using System;

namespace TestingControllersSample.ClientModels;

public class IdeaResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}