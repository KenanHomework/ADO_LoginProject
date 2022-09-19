using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_Lesson_1.Querys
{
    public abstract class QuerysStorage
    {
        public static string CheckDB = @"
                    USE [master];
                    
                    IF db_id('WolfTaxiDB') IS  NULL
                    BEGIN
                    	create database WolfTaxiDB;
                    END";

        public static string BackUpDifferent = @"
                    BACKUP DATABASE [WolfTaxiDB]
                    TO DISK = 'C:\Users\Public\StudentsDB.bak'
                    WITH DIFFERENTIAL,NAME = N'Full_Backup_WolfTaxiDB';";

        public static string BackUpFull = @"
                    BACKUP DATABASE [WolfTaxiDB]
                    TO DISK = 'C:\Users\Public\WolfTaxiDB.bak'
                    WITH NAME = N'Full_Backup_WolfTaxiDB';";

        public static string CreateTable = @"
                    USE [WolfTaxiDB]
                    IF OBJECT_ID('Users', 'U') IS NULL
                    BEGIN
                        CREATE TABLE Users (
                        		Id INT PRIMARY KEY IDENTITY(0,1),
                        		[Username]   NVARCHAR(20)   not null UNIQUE,
                        		[Password]   NVARCHAR(MAX)  not null,
                        		[Phone]		 NVARCHAR(10)   not null,
                        		[Email]		 NVARCHAR(40)   not null,
                        		[SaltStr1]   NVARCHAR(MAX)  not null,
                        		[SaltStr2]   NVARCHAR(MAX)  not null,
                        		[Key]		 NCHAR	        not null,
                        )
                    END
                    ELSE
                    BEGIN
                    	DROP TABLE Users
                        CREATE TABLE Users (
                        		Id INT PRIMARY KEY IDENTITY(0,1),
                        		[Username]   NVARCHAR(20)   not null UNIQUE,
                        		[Password]   NVARCHAR(MAX)  not null,
                        		[Phone]		 NVARCHAR(10)   not null,
                        		[Email]		 NVARCHAR(40)   not null,
                        		[SaltStr1]   NVARCHAR(MAX)  not null,
                        		[SaltStr2]   NVARCHAR(MAX)  not null,
                        		[Key]		 CHAR	        not null,
                        )
                    END";

        public static string CheckTable = @"
                    USE [WolfTaxiDB];
                    
                    INSERT INTO Users VALUES(
                    'aaaaaaaaaaaaaaaaaaaa',
                    'aaaaaaaaaaaaaaaaaaaa',
                    'aaaaaaaaaa',
                    'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa',
                    'assssssssssssssssssssssssssssssssssssasasassssssssssssssssssssssssssss',
                    'assssssssssssssssssssssssssssssssssssasasassssssssssssssssssssssssssss',
                    'k')
                    
                    DECLARE @last_id INT =  (
                    							SELECT	
                    								CASE
                    									WHEN (SELECT TOP(1) Id FROM Users ORDER BY Id DESC) IS NULL THEN 0
                                                        ELSE (SELECT TOP(1) Id FROM Users ORDER BY Id DESC) 
                                                    END
                    						)
                    
                    DELETE FROM Users WHERE Id = @last_id";
    }
}
