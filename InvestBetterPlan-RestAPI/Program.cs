using InvestBetterPlan_RestAPI.Models;
using InvestBetterPlan_RestAPI.Repository;
using InvestBetterPlan_RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<challengeContext>(options =>
                        options.UseNpgsql(builder.Configuration.GetConnectionString("ChallengeDB"))
                        );
//Add registro el repositorio 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISummaryRepository, SummaryRepository>();
builder.Services.AddScoped<IGoalsRepository, GoalsRepository>();

builder.Services.AddControllers(option => {
    //option.ReturnHttpNotAcceptable = true;  //Al implementar logger esta linea puede ser comentada
    }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


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

app.MapControllers();

app.Run();
