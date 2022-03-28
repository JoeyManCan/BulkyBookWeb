using BulkyBook.DataAccess.Repositories.IRepositories;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
//using NUnit.Framework;

namespace BulkyBook.Tests
{

    public class CategoryRepositoryTest :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public CategoryRepositoryTest(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory.WithWebHostBuilder(config =>
                config.ConfigureAppConfiguration((context, builder) =>
                    {
                        //using config settings
                        builder.AddJsonFile("appsettings.json", optional: false,
                            reloadOnChange: false)
                            .AddJsonFile("appsettings.LocalDevelopment.json",
                                optional: true, reloadOnChange: true);
                    }
                )
            );
        }
        [Theory]
        [MemberData(nameof(Controller))]
        [Trait("Category", "Integration")]
        public void It_should_be_able_to_create_all_controllers(Type controller)
        {
            // Arrange
            var serviceScope = _applicationFactory.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            // act
            var service = serviceProvider.GetRequiredService(controller);

            // Assert
            //service.Should().NotBeNull($"controllers of type {controller.Name} can be created");

            Assert.NotNull(service);
        }
    }
}