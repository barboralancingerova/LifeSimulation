using GameOfLife.Components;

#if DEBUG
var grid = new Grid(Config.GridWidth, Config.GridHeight); 
grid.InitializePopulation(Config.ProducerChance, Config.HerbivoreChance, Config.PredatorChance);

int stepNumber = 0;
while (true)
{
    grid.Step();
    stepNumber++;
    grid.PrintStatus(stepNumber);
    Console.WriteLine("Stiskni Enter pro další krok, nebo napiš 'q' pro ukončení testu.");
    var input = Console.ReadLine();
    if (input == "q") break;
}
return;
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


