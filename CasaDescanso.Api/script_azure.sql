-- Creación de tablas para Casa Descanso (Versión Azure SQL)

CREATE TABLE DietTypes (
    Id int NOT NULL IDENTITY(1,1),
    Name nvarchar(100) NOT NULL,
    Description nvarchar(MAX) NULL,
    CONSTRAINT PK_DietTypes PRIMARY KEY (Id)
);

CREATE TABLE Residents (
    Id int NOT NULL IDENTITY(1,1),
    FirstName nvarchar(100) NOT NULL,
    LastName nvarchar(100) NOT NULL,
    MiddleName nvarchar(100) NOT NULL,
    BirthDate datetime2(6) NOT NULL,
    Gender nvarchar(20) NOT NULL,
    NSS nvarchar(20) NOT NULL,
    PhotoPath nvarchar(255) NULL,
    EmergencyContactName nvarchar(150) NOT NULL,
    EmergencyContactPhone nvarchar(20) NOT NULL,
    EmergencyContactRelation nvarchar(100) NOT NULL,
    SecondContactName nvarchar(150) NULL,
    SecondContactPhone nvarchar(20) NULL,
    DiagnosedDiseases nvarchar(MAX) NULL,
    Allergies nvarchar(MAX) NULL,
    BloodType nvarchar(5) NOT NULL,
    AdmissionDate datetime2(6) NOT NULL,
    Observations nvarchar(MAX) NULL,
    IsActive bit NOT NULL,
    CreatedAt datetime2(6) NOT NULL,
    CONSTRAINT PK_Residents PRIMARY KEY (Id)
);

CREATE TABLE Roles (
    Id int NOT NULL IDENTITY(1,1),
    Name nvarchar(50) NOT NULL,
    Description nvarchar(150) NULL,
    IsActive bit NOT NULL,
    CreatedAt datetime2(6) NOT NULL,
    CONSTRAINT PK_Roles PRIMARY KEY (Id)
);

CREATE TABLE Shifts (
    Id int NOT NULL IDENTITY(1,1),
    Name nvarchar(50) NOT NULL,
    StartTime time(7) NOT NULL,
    EndTime time(7) NOT NULL,
    CONSTRAINT PK_Shifts PRIMARY KEY (Id)
);

CREATE TABLE ResidentDiets (
    Id int NOT NULL IDENTITY(1,1),
    ResidentId int NOT NULL,
    DietTypeId int NOT NULL,
    StartDate datetime2(6) NOT NULL,
    EndDate datetime2(6) NULL,
    CONSTRAINT PK_ResidentDiets PRIMARY KEY (Id),
    CONSTRAINT FK_ResidentDiets_DietTypes_DietTypeId FOREIGN KEY (DietTypeId) REFERENCES DietTypes (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ResidentDiets_Residents_ResidentId FOREIGN KEY (ResidentId) REFERENCES Residents (Id) ON DELETE CASCADE
);

CREATE TABLE Workers (
    Id int NOT NULL IDENTITY(1,1),
    FirstName nvarchar(100) NOT NULL,
    LastName nvarchar(100) NOT NULL,
    MiddleName nvarchar(100) NOT NULL,
    BirthDate datetime2(6) NOT NULL,
    Gender nvarchar(20) NOT NULL,
    PhotoPath nvarchar(255) NULL,
    Phone nvarchar(20) NOT NULL,
    Email nvarchar(150) NULL,
    EmergencyContactName nvarchar(150) NOT NULL,
    EmergencyContactPhone nvarchar(20) NOT NULL,
    RFC nvarchar(13) NULL,
    CURP nvarchar(18) NULL,
    NSS nvarchar(20) NULL,
    RoleId int NOT NULL,
    EducationLevel nvarchar(100) NOT NULL,
    Allergies nvarchar(MAX) NULL,
    ShiftId int NOT NULL,
    IsActive bit NOT NULL,
    CreatedAt datetime2(6) NOT NULL,
    CONSTRAINT PK_Workers PRIMARY KEY (Id),
    CONSTRAINT FK_Workers_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Workers_Shifts_ShiftId FOREIGN KEY (ShiftId) REFERENCES Shifts (Id) ON DELETE CASCADE
);

CREATE TABLE UserAccounts (
    Id int NOT NULL IDENTITY(1,1),
    WorkerId int NOT NULL,
    Username nvarchar(50) NOT NULL,
    PasswordHash nvarchar(255) NOT NULL,
    RoleId int NOT NULL,
    IsActive bit NOT NULL,
    CreatedAt datetime2(6) NOT NULL,
    CONSTRAINT PK_UserAccounts PRIMARY KEY (Id),
    CONSTRAINT FK_UserAccounts_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserAccounts_Workers_WorkerId FOREIGN KEY (WorkerId) REFERENCES Workers (Id) ON DELETE CASCADE
);

CREATE TABLE Attendance (
    Id int NOT NULL IDENTITY(1,1),
    UserId int NOT NULL,
    CheckIn datetime2(6) NOT NULL,
    CheckOut datetime2(6) NULL,
    Date datetime2(6) NOT NULL,
    Notes nvarchar(MAX) NULL,
    Status nvarchar(MAX) NOT NULL,
    CONSTRAINT PK_Attendance PRIMARY KEY (Id),
    CONSTRAINT FK_Attendance_UserAccounts_UserId FOREIGN KEY (UserId) REFERENCES UserAccounts (Id) ON DELETE CASCADE
);

CREATE TABLE Incidents (
    Id int NOT NULL IDENTITY(1,1),
    ResidentId int NOT NULL,
    RegisteredByUserId int NOT NULL,
    Date datetime2(6) NOT NULL,
    Type nvarchar(MAX) NOT NULL,
    SeverityLevel nvarchar(MAX) NOT NULL,
    Description nvarchar(MAX) NOT NULL,
    CreatedAt datetime2(6) NOT NULL,
    CONSTRAINT PK_Incidents PRIMARY KEY (Id),
    CONSTRAINT FK_Incidents_Residents_ResidentId FOREIGN KEY (ResidentId) REFERENCES Residents (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Incidents_UserAccounts_RegisteredByUserId FOREIGN KEY (RegisteredByUserId) REFERENCES UserAccounts (Id) ON DELETE CASCADE
);

CREATE TABLE VitalSigns (
    Id int NOT NULL IDENTITY(1,1),
    ResidentId int NOT NULL,
    RecordedByUserId int NOT NULL,
    Temperature decimal(18,2) NULL,
    BloodPressure nvarchar(MAX) NULL,
    HeartRate int NULL,
    OxygenSaturation decimal(18,2) NULL,
    GlucoseLevel decimal(18,2) NULL,
    Weight decimal(18,2) NULL,
    Notes nvarchar(MAX) NULL,
    RecordedAt datetime2(6) NOT NULL,
    RespiratoryFrequency int NULL,
    CONSTRAINT PK_VitalSigns PRIMARY KEY (Id),
    CONSTRAINT FK_VitalSigns_Residents_ResidentId FOREIGN KEY (ResidentId) REFERENCES Residents (Id) ON DELETE CASCADE,
    CONSTRAINT FK_VitalSigns_UserAccounts_RecordedByUserId FOREIGN KEY (RecordedByUserId) REFERENCES UserAccounts (Id) ON DELETE CASCADE
);

-- Índices
CREATE INDEX IX_Attendance_UserId ON Attendance (UserId);
CREATE INDEX IX_Incidents_RegisteredByUserId ON Incidents (RegisteredByUserId);
CREATE INDEX IX_Incidents_ResidentId ON Incidents (ResidentId);
CREATE INDEX IX_ResidentDiets_DietTypeId ON ResidentDiets (DietTypeId);
CREATE INDEX IX_ResidentDiets_ResidentId ON ResidentDiets (ResidentId);
CREATE INDEX IX_UserAccounts_RoleId ON UserAccounts (RoleId);
CREATE UNIQUE INDEX IX_UserAccounts_WorkerId ON UserAccounts (WorkerId);
CREATE INDEX IX_VitalSigns_RecordedByUserId ON VitalSigns (RecordedByUserId);
CREATE INDEX IX_VitalSigns_ResidentId ON VitalSigns (ResidentId);
CREATE INDEX IX_Workers_RoleId ON Workers (RoleId);
CREATE INDEX IX_Workers_ShiftId ON Workers (ShiftId);