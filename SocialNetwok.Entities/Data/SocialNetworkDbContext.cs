using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwok.Entities.Entities;

namespace SocialNetwok.Entities.Data;

public class SocialNetworkDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
{
    public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options) : base(options) { }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<FriendRequest>()
			.HasOne(fr => fr.Sender)
			.WithMany(u => u.FriendRequests)
			.HasForeignKey(fr => fr.SenderId)
			.OnDelete(DeleteBehavior.Cascade);  


		modelBuilder.Entity<FriendRequest>()
			.HasOne(fr => fr.Receiver)
			.WithMany()  
			.HasForeignKey(fr => fr.ReceiverId)
			.OnDelete(DeleteBehavior.Restrict); 

		modelBuilder.Entity<Friend>()
			.HasKey(f => f.Id);

		modelBuilder.Entity<Friend>()
			.HasOne(f => f.Own)  
			.WithMany(u => u.Friends)  
			.HasForeignKey(f => f.OwnId)
			.OnDelete(DeleteBehavior.Cascade);  

		modelBuilder.Entity<Friend>()
			.HasOne(f => f.YourFriend)  
			.WithMany()  
			.HasForeignKey(f => f.YourFriendId)
			.OnDelete(DeleteBehavior.Restrict);  


		modelBuilder.Entity<Chat>()
			.HasOne(c => c.User1)
			.WithMany(u => u.Chats)  
			.HasForeignKey(c => c.User1Id)
			.OnDelete(DeleteBehavior.Cascade);  


		modelBuilder.Entity<Chat>()
			.HasOne(c => c.User2)
			.WithMany()
			.HasForeignKey(c => c.User2Id)
			.OnDelete(DeleteBehavior.Restrict);  


		modelBuilder.Entity<Message>()
			.HasOne(m => m.Chat)
			.WithMany(c => c.Messages)
			.HasForeignKey(m => m.ChatId)
			.OnDelete(DeleteBehavior.Cascade);


		modelBuilder.Entity<Notification>()
			.HasKey(n => n.Id); 


		modelBuilder.Entity<Notification>()
			.HasOne(n => n.Receiver)  
			.WithMany(u => u.ReceivedNotifications)  
			.HasForeignKey(n => n.ReceiverId)  
			.OnDelete(DeleteBehavior.Cascade);  

		modelBuilder.Entity<Notification>()
			.HasOne(n => n.Sender)  
			.WithMany(u => u.SentNotifications)  
			.HasForeignKey(n => n.SenderId) 
			.OnDelete(DeleteBehavior.Restrict);  
	}

	public DbSet<Chat> Chats { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }
    public DbSet<LikeComment> LikesComments { get; set; }
    public DbSet<LikePost> LikesPosts { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Post> Posts { get; set; }
}
