using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // [5] builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AgentsDB"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/ListAgents", async (AppDbContext db) => await db.Agents.ToListAsync());

app.MapGet("/ListAgentById", async (int id, AppDbContext db) =>
                             await db.Agents.FindAsync(id) is Agent agent ? Results.Ok(agent) : Results.NotFound("The agent you are looking for doesnt seems to be in our system")) ;

app.MapGet("/ListAgents/Active", async (AppDbContext db) =>
                                   await db.Agents.Where(a => a.IsActive).ToListAsync());

app.MapPost("/CreateAgent", async (AppDbContext db, Agent agent) =>
{
    db.Agents.Add(agent);
    await db.SaveChangesAsync();
    return Results.Created($"/Agents/{agent.Id}",agent);
} );

app.MapPut("/AlterAgent", async (int id, Agent agentInput, AppDbContext db) =>
{
    var agent = await db.Agents.FindAsync(id);

    if (agent is null)
        return Results.NotFound("The agent your trying to edit doesnt seem to be in our system...");

    agent.Name= agentInput.Name;
    agent.IsActive=agentInput.IsActive;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/DeleteAgent", async (int id, AppDbContext db) =>
{
    if(await db.Agents.FindAsync(id) is Agent agent)
    {
        db.Agents.Remove(agent);
        await db.SaveChangesAsync();
        return Results.Ok(agent);
    }
    return Results.NotFound("The agent your looking for doesnt seem to exist in our database...");
});

app.Run();

class Agent
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; } 

}

class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions <AppDbContext> options) : base(options) 
    { }

    public DbSet<Agent> Agents => Set <Agent> ();
}