using EventSourcingApplication.Shared.Services;
using EventSourcingApplication.Shared.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEventStoreService, EventStoreService>();
builder.Services.AddScoped<IMongoDbService, MongoDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Products}/{action=CreateProduct}/{id?}")
    .WithStaticAssets();


app.Run();