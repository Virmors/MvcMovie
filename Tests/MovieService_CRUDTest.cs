using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MvcMovie.Data;
using MvcMovie.Features.Movies.Services;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class MovieService_CRUDTest
{
    private static MovieService CreateService()
    {
        var dbCtxBuilder = new DbContextOptionsBuilder<MvcMovieContext>();
        var inMemDb = dbCtxBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = inMemDb.Options;
        var context = new MvcMovieContext(options);

        return new MovieService(context);
    }
    
    [Fact]
    public async Task MovieService_CanPerformCRUDOp()
    {
        var svc = CreateService();

        var movie = new Movie
        {
            Title = "Inception",
            Genre = "Sci-Fi",
            Price = 18.99M,
            Rating = "PG-13",
            ReleaseDate = DateTime.Parse("2010-07-16")
        };

        await svc.AddAsync(movie);

        var read = await svc.GetByIdAsync(movie.Id);
        Assert.NotNull(read);
        Assert.Equal("Inception", read.Title);

        read.Title = "Inception (20th year anniversary edition)";
        await svc.UpdateAsync(read);
        var updated = await svc.GetByIdAsync(movie.Id);
        Assert.Equal("Inception (20th year anniversary edition)", updated.Title);

        await svc.DeleteAsync(movie.Id);
        var deleted = await svc.GetByIdAsync(movie.Id);
        Assert.Null(deleted);
    }
}
