using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleNote.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleNote.Api.Data
{
    public class DbInitializer
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                MyContext myContext = serviceScope.ServiceProvider.GetService<MyContext>();

                {
                    if (!myContext.Notes.Any())
                    {
                        myContext.Notes.AddRange(
                            new List<Note>
                            {
                                new Note{
                                    Title = "Title 1",
                                    Body = "Body 1 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "John",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 2",
                                    Body = "Body 2 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "John",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 3",
                                    Body = "Body 3 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "Kim",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 4",
                                    Body = "Body 4 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "Lisa",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 5",
                                    Body = "Body 5 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "Dave",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 6",
                                    Body = "Body 6 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "John",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 7",
                                    Body = "Body 7 Quickly capture what's on your mind and get a reminder later at the right place or time",
                                    Username = "Kim",
                                    LastModified = DateTime.Now
                                },
                                new Note{
                                    Title = "Title 8",
                                    Body = "Body 8 Quickly capture what's on your mind and get a reminder later at the right place or time ",
                                    Username = "Lisa",
                                    LastModified = DateTime.Now
                                }
                            }
                        );
                        await myContext.SaveChangesAsync();
                    }
                }
            }
            
        }
    }
}
