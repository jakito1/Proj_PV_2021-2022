select * from dbo.AspNetRoles;
select * from dbo.AspNetUserRoles;
select * from dbo.AspNetUsers;

select * from dbo.Client;
select * from dbo.Photos;
select * from dbo.Trainer;
select * from dbo.Nutritionist;
select * from dbo.TrainingPlan;
select * from dbo.NutritionPlan;

select * from dbo.Exercise;
delete from dbo.Photos;
update dbo.Client set GymId = NULL where ClientId = 1;
insert into dbo.Client (ClientId, Weight, Height) values (1, 100, 100);
/*delete from dbo.AspNetUsers;*/
/*select * from dbo.AspNetUserRoles;*/
SET IDENTITY_INSERT [dbo].[Client] off
/*Admin (admin@admin.pt) Password: 4p^91S!Mpu&tZgrfmiA^fWT&L */
/*Gym (apenas para testes) (gym@gym.pt) Password: 4p^91S!Mpu&tZgrfmiA^fWT&L */
/* /Trainers/EditTrainerSettings/trainer */