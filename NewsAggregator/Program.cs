using RssFeedAggregator.DAL;
using RssFeedAggregator.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfig(builder.Configuration);
builder.Services.AddDatabaseContext(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddUnitOfWork();
builder.Services.AddScheduledJobs(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddBllServices();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddHttpClientRetryPolicy(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
