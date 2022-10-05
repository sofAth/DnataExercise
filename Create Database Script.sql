CREATE TABLE Movies(ID INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Genre TEXT, YearOfRelease INTEGER, RunningTime INTEGER);
CREATE TABLE Users(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT);
CREATE TABLE UserRatings(MovieID INTEGER, UserID INTEGER, Rating INTEGER);
CREATE TABLE UserRatings2(MovieID INTEGER, UserID INTEGER, Rating INTEGER, PRIMARY KEY(MovieID, UserID));
select * from Movies
select * from Users u 
select * from UserRatings ur order by MovieID, UserID, Rating 
alter table UserRatings2 rename to UserRatings

drop table UserRatings 
insert into UserRatings2 select * from UserRatings 

insert into Movies(Title, Genre, YearOfRelease, RunningTime) values("Armageddon", "Adventure", "1998", "151");

insert into Users(Name) values ("Joanne")

insert into UserRatings (MovieID, UserID, Rating) values(6, 2, 2)

select MovieID , avg(Rating) Rating
from UserRatings ur 
where ur.UserID = 2
group by MovieID 
order by Rating DESC 