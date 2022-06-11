# UserManagement
Kullanılan paketler;
	Microsoft.AspNetCore.Identity.EntityFrameworkCore,
	Microsoft.EntityFrameworkCore,
	Microsoft.EntityFrameworkCore.Desing,
	Microsoft.EntityFrameworkCore.SqlServer,
	Microsoft.EntityFrameworkCore.Tools

appsetting.json dosyasına "DbConnection" tag'ı altına SqlServer bağlantı textimizi ekledik. SqlServerimizde halihazırda UserManagement
	adlı database oluşturmuştuk.
Migration yöntemi ile database'imizi oluşturmak için Araçlar > Nuget Paket Yönetici > Paket Yöneticisi Konsolu'nu açıyoruz.
	Paket olarak eklediğimiz "AspNetCore.Identity.EntityFrameworkCore" ve paket içerisindeki sınıflardan kalıtım olarak aldığımız
	"AppIdentityUser","AppIdentityRole","AppIdentityDbContext" sınıfları içerisindeki propertylerin sql servere otomatik eklenmesi
	için migration kullanıyoruz.
	
	Paket Yöneticisi Konsolu'na;
		1. add-migration initialcreate 
			Daha önce vermiş olduğumuz SqlServer bağlantısını kullanarak tabloların Query'sini oluşturur.
		2. update-database
			Oluşturulan Query'i çalışır ve database üzerinde tablolar oluşur.
