using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PromiDb>(opt => opt.UseInMemoryDatabase("PromiList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

RouteGroupBuilder PromiItems = app.MapGroup("/Promiitems");

PromiItems.MapGet("/", GetAllPromis);
PromiItems.MapGet("/complete", GetCompletePromis);
PromiItems.MapGet("/{id}", GetPromi);
PromiItems.MapPost("/", CreatePromi);
PromiItems.MapPut("/{id}", UpdatePromi);
PromiItems.MapDelete("/{id}", DeletePromi);

app.Run();

static async Task<IResult> GetAllPromis(PromiDb db)
{
    return TypedResults.Ok(await db.Promis.Select(x => new PromiItemDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetCompletePromis(PromiDb db) {
    return TypedResults.Ok(await db.Promis.Where(t => t.IsManager).Select(x => new PromiItemDTO(x)).ToListAsync());
}

static async Task<IResult> GetPromi(int id, PromiDb db)
{
    return await db.Promis.FindAsync(id)
        is Promi Promi
            ? TypedResults.Ok(new PromiItemDTO(Promi))
            : TypedResults.NotFound();
}

static async Task<IResult> CreatePromi(PromiItemDTO PromiItemDTO, PromiDb db)
{
    var PromiItem = new Promi
    {
        IsManager = PromiItemDTO.IsManager,
        FName = PromiItemDTO.FName
    };

    db.Promis.Add(PromiItem);
    await db.SaveChangesAsync();

    PromiItemDTO = new PromiItemDTO(PromiItem);

    return TypedResults.Created($"/Promiitems/{PromiItem}", PromiItemDTO);
}

static async Task<IResult> UpdatePromi(int id, PromiItemDTO PromiItemDTO, PromiDb db)
{
    var Promi = await db.Promis.FindAsync(id);

    if (Promi is null) return TypedResults.NotFound();

    Promi.FName = PromiItemDTO.FName;
    Promi.IsManager = PromiItemDTO.IsManager;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeletePromi(int id, PromiDb db)
{
    if (await db.Promis.FindAsync(id) is Promi Promi)
    {
        db.Promis.Remove(Promi);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}