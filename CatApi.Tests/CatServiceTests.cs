using System.Resources;
using AutoMapper;
using CatApi.MapperProfiles;
using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services;
using CatApi.Services.Database;
using CatApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CatApi.Tests;

public class CatServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IHttpRequestService> _mockHttpRequestService;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ResourceManager> _mockResourceManager;
    private readonly Mock<ICatRepo> _mockCatRepo;
    private readonly Mock<ITagService> _mockTagService;
    private readonly Mock<ICatTagService> _mockCatTagService;
    private readonly CatService _catService;
    private readonly IMapper _mapper;
    private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;
    private readonly Mock<IServiceScope> _mockServiceScope;
    private readonly Mock<CatDbContext> _mockCatDbContext;

    
    public CatServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpRequestService = new Mock<IHttpRequestService>();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockResourceManager = new Mock<ResourceManager>();
        _mockCatRepo = new Mock<ICatRepo>();
        _mockTagService = new Mock<ITagService>();
        _mockCatTagService = new Mock<ICatTagService>();
        _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceScope = new Mock<IServiceScope>();

        //Actual mappings
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingSourceCatProfile());
            mc.AddProfile(new MappingEntityCatProfile());
        });

        _mapper = mappingConfig.CreateMapper();

        //Moq ef core DBContextS
        var options = new DbContextOptionsBuilder<CatDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;

        _mockCatDbContext = new Mock<CatDbContext>(options);

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICatRepo))).Returns(_mockCatRepo.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ITagService))).Returns(_mockTagService.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ICatTagService))).Returns(_mockCatTagService.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IHttpRequestService))).Returns(_mockHttpRequestService.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IMapper))).Returns(_mapper);

        _catService = new CatService(
            _mockServiceProvider.Object,
            _mockConfiguration.Object,
            _mockResourceManager.Object
        );
    }
    [Fact]
    public async Task FetchCatsAsync_ShouldThrowException_WhenApiKeyIsMissing()
    {        
        _mockConfiguration.Setup(c => c.GetSection("CatApiSettings:CatApiKey").Value).Returns(string.Empty);
        _mockResourceManager.Setup(r => r.GetString("CatApiKeyNotFound")).Returns("API key not found");

        var exception = await Assert.ThrowsAsync<Exception>(() => _catService.FetchCatsAsync());
        Assert.Equal("API key not found", exception.Message);
    }

    [Fact]
    public async Task FetchCatsAsync_ShouldThrowException_WhenResponseIsNull()
    {
        _mockConfiguration.Setup(c => c.GetSection("CatApiSettings:CatApiKey").Value).Returns("valid-api-key");
        _mockHttpRequestService.Setup(h => h.HttpGet(It.IsAny<Dictionary<string, string>>(), It.IsAny<string>())).ReturnsAsync(string.Empty);
        _mockResourceManager.Setup(r => r.GetString("CatNotFetched")).Returns("Cats not fetched");

        var exception = await Assert.ThrowsAsync<Exception>(() => _catService.FetchCatsAsync());
        Assert.Equal("Cats not fetched", exception.Message);
    }

    [Fact]
    public async Task FetchCatsAsync_ShouldReturnFetchCount_WhenSuccessful()
    {
        var catApiKey = "valid-api-key";
        var catApiUrl = "https://test.com";
        var httpResponse = @"[{'id':'1','url':'https://test.com/1.jpg'}]";

        _mockConfiguration.Setup(c => c.GetSection("CatApiSettings:CatApiKey").Value).Returns(catApiKey);
        _mockConfiguration.Setup(c => c.GetSection("CatApiSettings:CatApiUrl").Value).Returns(catApiUrl);
        _mockHttpRequestService.Setup(h => h.HttpGet(It.IsAny<Dictionary<string, string>>(), catApiUrl)).ReturnsAsync(httpResponse);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(_mockServiceScopeFactory.Object);
        _mockServiceScopeFactory.Setup(sf => sf.CreateScope()).Returns(_mockServiceScope.Object);
        _mockServiceScope.Setup(s => s.ServiceProvider).Returns(_mockServiceProvider.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(CatDbContext))).Returns(_mockCatDbContext.Object);

        var cats = new List<CatEntity> { new CatEntity { CatId = "1", Image = "https://test.com/1.jpg" } };
        _mockCatRepo.Setup(r => r.StoreCats(It.IsAny<IEnumerable<CatEntity>>(), It.IsAny<CatDbContext>())).Returns(Task.CompletedTask);
        _mockTagService.Setup(t => t.StoreTags(It.IsAny<IEnumerable<TagEntity>>(), It.IsAny<CatDbContext>())).Returns(Task.CompletedTask);
        _mockCatTagService.Setup(ct => ct.StoreTags(It.IsAny<IEnumerable<CatTagEntity>>(), It.IsAny<CatDbContext>())).Returns(Task.CompletedTask);

        var result = await _catService.FetchCatsAsync();

        Assert.Equal(1, result);
    }
}
