using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();



app.Run();

class Agent
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsActived { get; set; } 

}

class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions <AppDbContext> options) : base(options) 
    { }

    public DbSet<Agent> Agents => Set <Agent> ();
}