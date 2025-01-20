using CaminoManager.Data.Models;
using CaminoManager.ServiceDefaults.DTOs;
using CaminoManager.ApiService.Mappers;
using Microsoft.EntityFrameworkCore;
using CaminoManager.Data.Contexts;

namespace CaminoManager.ApiService.Endpoints;

public static class BrotherEndpoints
{
    private static readonly BrotherMapper _mapper = new();

    public static RouteGroupBuilder MapBrotherEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/brothers").WithTags("Brothers");

        group.MapGet("/", async (CaminoManagerDbContext db) =>
        {
            var brothers = await db.Set<Brother>().ToListAsync();
            return brothers.Select(_mapper.ToDto);
        });

        group.MapGet("/{personId}/{communityId}", async (Guid personId, Guid communityId, CaminoManagerDbContext db) =>
        {
            var brother = await db.Set<Brother>()
                .FirstOrDefaultAsync(b => b.PersonId == personId && b.CommunityId == communityId);
            
            return brother is null ? Results.NotFound() : Results.Ok(_mapper.ToDto(brother));
        });

        group.MapPost("/", async (CreateBrotherDto dto, CaminoManagerDbContext db) =>
        {
            var brother = _mapper.ToEntity(dto);
            db.Set<Brother>().Add(brother);
            await db.SaveChangesAsync();
            
            return Results.Created($"/brothers/{brother.PersonId}/{brother.CommunityId}", _mapper.ToDto(brother));
        });

        group.MapPut("/{personId}/{communityId}", async (Guid personId, Guid communityId, UpdateBrotherDto dto, CaminoManagerDbContext db) =>
        {
            var brother = await db.Set<Brother>()
                .FirstOrDefaultAsync(b => b.PersonId == personId && b.CommunityId == communityId);
            
            if (brother is null) return Results.NotFound();
            
            _mapper.UpdateEntity(dto, brother);
            await db.SaveChangesAsync();
            
            return Results.Ok(_mapper.ToDto(brother));
        });

        group.MapDelete("/{personId}/{communityId}", async (Guid personId, Guid communityId, CaminoManagerDbContext db) =>
        {
            var brother = await db.Set<Brother>()
                .FirstOrDefaultAsync(b => b.PersonId == personId && b.CommunityId == communityId);
            
            if (brother is null) return Results.NotFound();
            
            db.Set<Brother>().Remove(brother);
            await db.SaveChangesAsync();
            
            return Results.NoContent();
        });

        return group;
    }
}