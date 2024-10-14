using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwok.DataAccess.Abstraction;
using SocialNetwok.DataAccess.Concrete.EFEntityFramework;
using SocialNetwok.Entities.Data;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Business.Concrete;
using SocialNetwork.WebUI.Hubs;
using SocialNetwork.WebUI.Services.Abstract;
using SocialNetwork.WebUI.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SocialNetworkDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<CustomIdentityUser, CustomIdentityRole>()
    .AddEntityFrameworkStores<SocialNetworkDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IChatDAL, EFChatDAL>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<ICommentDAL, EFCommentDAL>();

builder.Services.AddScoped<IFriendDAL, EFFriendDAL>();
builder.Services.AddScoped<IFriendService, FriendService>();

builder.Services.AddScoped<IFriendRequestDAL, EFFriendRequestDAL>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();

builder.Services.AddScoped<ILikeCommentDAL, EFLikeCommentDAL>();

builder.Services.AddScoped<ILikePostDAL, EFLikePostDAL>();

builder.Services.AddScoped<IMessageDAL, EFMessageDAL>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<INotificationsDAL, EFNotificationDAL>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddScoped<IPostDAL, EFPostDAL>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<IUserDAL, EFUserDAL>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPhotoService, PhotoService>();


builder.Services.AddSignalR();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SocialNetworkHub>("/socialNetworkHub");

app.Run();
