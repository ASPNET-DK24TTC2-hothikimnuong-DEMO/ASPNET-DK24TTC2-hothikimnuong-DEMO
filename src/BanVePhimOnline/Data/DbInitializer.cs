using BanVePhimOnline.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BanVePhimOnline.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Seed Roles
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("User")).Wait();
            }

            // Seed Users
            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@movie.com",
                    Email = "admin@movie.com",
                    FullName = "Administrator",
                    Address = "Admin HQ",
                    EmailConfirmed = true
                };
                var resultAdmin = userManager.CreateAsync(adminUser, "Admin@123").Result;
                if (resultAdmin.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }

                var normalUser = new ApplicationUser
                {
                    UserName = "user@movie.com",
                    Email = "user@movie.com",
                    FullName = "Nguyen Van A",
                    Address = "Sai Gon",
                    EmailConfirmed = true
                };
                var resultUser = userManager.CreateAsync(normalUser, "User@123").Result;
                if (resultUser.Succeeded)
                {
                    userManager.AddToRoleAsync(normalUser, "User").Wait();
                }
            }

            // Check if we need to re-seed movies (if the first movie is "The Shawshank Redemption", we assume it's old data)
            if (context.Movies.Any(m => m.Title == "The Shawshank Redemption"))
            {
                // Clear existing data to re-seed
                var oldBookings = context.Bookings.ToList();
                context.Bookings.RemoveRange(oldBookings);
                
                var oldShowtimes = context.Showtimes.ToList();
                context.Showtimes.RemoveRange(oldShowtimes);

                var oldCinemaHalls = context.CinemaHalls.ToList();
                context.CinemaHalls.RemoveRange(oldCinemaHalls);

                var oldCinemas = context.Cinemas.ToList();
                context.Cinemas.RemoveRange(oldCinemas);

                var oldMovies = context.Movies.ToList();
                context.Movies.RemoveRange(oldMovies);

                context.SaveChanges();
            }

            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            var movies = new Movie[]
            {
                new Movie{Title="Mai", Description="Mai, một người phụ nữ 37 tuổi, phải vật lộn với cuộc sống khó khăn và quá khứ đầy sóng gió. Cô gặp Dương, một chàng trai trẻ, và tình yêu chớm nở giữa họ.", Director="Trấn Thành", Cast="Phương Anh Đào, Tuấn Trần", Genre="Tâm Lý, Tình Cảm", DurationMinutes=131, ReleaseDate=DateTime.Parse("2024-02-10"), PosterUrl="https://upload.wikimedia.org/wikipedia/vi/6/6f/Mai_2024_poster.jpg", TrailerUrl="https://www.youtube.com/watch?v=3gXz3gXz3gX", AgeRating="C18"},
                new Movie{Title="Lật Mặt 7: Một Điều Ước", Description="Câu chuyện về bà Hai, người mẹ đơn thân nuôi 5 người con khôn lớn. Khi bà gặp nạn, những mâu thuẫn gia đình bắt đầu nảy sinh.", Director="Lý Hải", Cast="Thanh Hiền, Trương Minh Cường", Genre="Gia Đình, Tâm Lý", DurationMinutes=138, ReleaseDate=DateTime.Parse("2024-04-26"), PosterUrl="https://upload.wikimedia.org/wikipedia/vi/4/4e/Lat_mat_7_poster.jpg", TrailerUrl="https://www.youtube.com/watch?v=4hY4hY4hY4h", AgeRating="P"},
                new Movie{Title="Đất Rừng Phương Nam", Description="Hành trình đi tìm cha của cậu bé An trong bối cảnh miền Nam Việt Nam những năm đầu thế kỷ 20, đầy biến động và hào hùng.", Director="Nguyễn Quang Dũng", Cast="Hạo Khang, Tuấn Trần, Trấn Thành", Genre="Phiêu Lưu, Lịch Sử", DurationMinutes=110, ReleaseDate=DateTime.Parse("2023-10-20"), PosterUrl="https://upload.wikimedia.org/wikipedia/vi/e/e6/%C4%90%E1%BA%A5t_r%E1%BB%ABng_ph%C6%B0%C6%A1ng_Nam_2023_poster.jpg", TrailerUrl="https://www.youtube.com/watch?v=5jZ5jZ5jZ5j", AgeRating="C13"},
                new Movie{Title="Nhà Bà Nữ", Description="Câu chuyện về gia đình bà Nữ làm nghề bán bánh canh cua. Những mâu thuẫn thế hệ và áp lực cuộc sống tạo nên những tình huống dở khóc dở cười.", Director="Trấn Thành", Cast="Lê Giang, Uyển Ân, Song Luân", Genre="Hài, Tâm Lý", DurationMinutes=102, ReleaseDate=DateTime.Parse("2023-01-22"), PosterUrl="https://upload.wikimedia.org/wikipedia/vi/3/3a/Nha_ba_nu_poster.jpg", TrailerUrl="https://www.youtube.com/watch?v=6kL6kL6kL6k", AgeRating="C16"},
                new Movie{Title="Bố Già", Description="Cuộc sống của ông Ba Sang làm nghề chở hàng thuê và cậu con trai Quắn, một YouTuber nổi tiếng. Tình cha con đầy cảm động và những bài học sâu sắc.", Director="Trấn Thành, Vũ Ngọc Đãng", Cast="Trấn Thành, Tuấn Trần", Genre="Hài, Gia Đình", DurationMinutes=128, ReleaseDate=DateTime.Parse("2021-03-12"), PosterUrl="https://upload.wikimedia.org/wikipedia/vi/thumb/9/9d/Bo_Gia_2021_poster.jpg/220px-Bo_Gia_2021_poster.jpg", TrailerUrl="https://www.youtube.com/watch?v=7mM7mM7mM7m", AgeRating="C13"}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();

            var cinemas = new Cinema[]
            {
                new Cinema{Name="CGV Vincom Đồng Khởi", Address="72 Lê Thánh Tôn, Quận 1, TP.HCM"},
                new Cinema{Name="Galaxy Nguyễn Du", Address="116 Nguyễn Du, Quận 1, TP.HCM"},
                new Cinema{Name="Lotte Cinema Nam Sài Gòn", Address="469 Nguyễn Hữu Thọ, Quận 7, TP.HCM"}
            };
            foreach (Cinema c in cinemas)
            {
                context.Cinemas.Add(c);
            }
            context.SaveChanges();

            var cinemaHalls = new CinemaHall[]
            {
                new CinemaHall{Name="Phòng 1", CinemaId=cinemas[0].Id, TotalSeats=50},
                new CinemaHall{Name="IMAX", CinemaId=cinemas[0].Id, TotalSeats=100},
                new CinemaHall{Name="Phòng A", CinemaId=cinemas[1].Id, TotalSeats=60},
                new CinemaHall{Name="Phòng B", CinemaId=cinemas[1].Id, TotalSeats=60},
                new CinemaHall{Name="Screen 1", CinemaId=cinemas[2].Id, TotalSeats=80}
            };
            foreach (CinemaHall ch in cinemaHalls)
            {
                context.CinemaHalls.Add(ch);
            }
            context.SaveChanges();

            var showtimes = new Showtime[]
            {
                new Showtime{MovieId=movies[0].Id, CinemaHallId=cinemaHalls[0].Id, StartTime=DateTime.Now.AddDays(1).AddHours(18), Price=120000},
                new Showtime{MovieId=movies[0].Id, CinemaHallId=cinemaHalls[0].Id, StartTime=DateTime.Now.AddDays(1).AddHours(21), Price=120000},
                new Showtime{MovieId=movies[1].Id, CinemaHallId=cinemaHalls[1].Id, StartTime=DateTime.Now.AddDays(1).AddHours(19), Price=180000},
                new Showtime{MovieId=movies[2].Id, CinemaHallId=cinemaHalls[2].Id, StartTime=DateTime.Now.AddDays(1).AddHours(20), Price=100000},
                new Showtime{MovieId=movies[3].Id, CinemaHallId=cinemaHalls[3].Id, StartTime=DateTime.Now.AddDays(2).AddHours(18), Price=100000},
                new Showtime{MovieId=movies[4].Id, CinemaHallId=cinemaHalls[4].Id, StartTime=DateTime.Now.AddDays(2).AddHours(19), Price=110000}
            };
            foreach (Showtime s in showtimes)
            {
                context.Showtimes.Add(s);
            }
            context.SaveChanges();
        }
    }
}
