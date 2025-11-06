CREATE DATABASE AutoAlertDB;
GO

USE AutoAlertDB;
GO

-- ============================================================
--  Script de creación de base de datos para AutoAlert
--  Autor: Aswin Niño
--  Descripción: Estructura relacional para gestión de alertas 
--  de servicios públicos con auditoría, roles, permisos y acceso
-- ============================================================


-- ============================================================
-- 1. Tabla: Groups
-- Contiene los grupos empresariales. Cada grupo puede tener
-- múltiples empresas asociadas.
-- ============================================================
CREATE TABLE Groups (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- ============================================================
-- 2. Tabla: Companies
-- Contiene las empresas asociadas a los grupos.
-- Cada empresa puede tener múltiples tiendas.
-- ============================================================
CREATE TABLE Companies (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    GroupId UNIQUEIDENTIFIER NULL,
    Name NVARCHAR(100) NOT NULL,
    NIT NVARCHAR(20) NULL,
    Address NVARCHAR(150),
    PhoneNumber NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);

-- ============================================================
-- 3. Tabla: Stores
-- Contiene las tiendas o puntos de venta de cada empresa.
-- ============================================================
CREATE TABLE Stores (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(150),
    City NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
);

-- ============================================================
-- 4. Tabla: DocumentTypes
-- Catálogo de tipos de documento (CC, NIT, CE, etc.)
-- ============================================================
CREATE TABLE DocumentTypes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL,
    Abbreviation NVARCHAR(10) NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- ============================================================
-- 5. Tabla: Roles
-- Define los diferentes roles del sistema (Admin, Usuario, etc.)
-- ============================================================
CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- ============================================================
-- 6. Tabla: Users
-- Contiene los usuarios del sistema, asociados a grupos y/o empresas.
-- ============================================================
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DocumentTypeId UNIQUEIDENTIFIER NULL,
    RoleId UNIQUEIDENTIFIER NULL,
    Names NVARCHAR(150) NOT NULL,
    LastNames NVARCHAR(150),
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255),
    Address NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    GoogleId NVARCHAR(255) NULL,
    DocumentNumber NVARCHAR(50) NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    FOREIGN KEY (DocumentTypeId) REFERENCES DocumentTypes(Id)
);

-- MÓDULOS
CREATE TABLE Modules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- SUBMÓDULOS
CREATE TABLE SubModules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ModuleId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    FOREIGN KEY (ModuleId) REFERENCES Modules(Id)
);

-- ROLES + MODULES
CREATE TABLE RoleSubModules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleId UNIQUEIDENTIFIER NOT NULL,
    SubModuleId UNIQUEIDENTIFIER NOT NULL,
    IsEnabled BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    FOREIGN KEY (SubModuleId) REFERENCES SubModules(Id)
);
-- PERMISOS PERSONALIZADOS (OVERRIDE)
CREATE TABLE UserSubmodules (
    UserId UNIQUEIDENTIFIER NOT NULL,
    SubModuleId UNIQUEIDENTIFIER NOT NULL,
    IsEnabled BIT DEFAULT 0,
    PRIMARY KEY (UserId, SubModuleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (SubModuleId) REFERENCES SubModules(Id)
);

-- ============================================================
-- 11. Tabla: Services
-- Contiene los servicios públicos asociados a las tiendas.
-- ============================================================
CREATE TABLE Services (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StoreId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Provider NVARCHAR(100),
    AccountNumber NVARCHAR(100),
    DueDate DATE NULL,
    Amount DECIMAL(18,2) NULL,
    Status NVARCHAR(50) DEFAULT 'Pendiente',
    LastCheck DATETIME NULL,
    AlertSent BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (StoreId) REFERENCES Stores(Id)
);

-- ============================================================
-- 12. Tabla: Alerts
-- Define las alertas automáticas programadas para los servicios.
-- ============================================================
CREATE TABLE Alerts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ServiceId UNIQUEIDENTIFIER NOT NULL,        -- Servicio relacionado a la alerta
    ScheduledAt DATETIME NOT NULL,              -- Fecha/hora programada de envío
    Channel NVARCHAR(50),                       -- Canal (WhatsApp, Email, SMS)
    Status NVARCHAR(50),                        -- Estado (Programada, Enviada, Fallida)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (ServiceId) REFERENCES Services(Id)
);

-- ============================================================
-- 13. Tabla: Notifications
-- Guarda los registros de alertas efectivamente enviadas.
-- ============================================================
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AlertId UNIQUEIDENTIFIER NOT NULL,          -- Alerta enviada
    UserId UNIQUEIDENTIFIER NOT NULL,           -- Usuario destinatario
    SentAt DATETIME,                            -- Fecha de envío real
    Result NVARCHAR(100),                       -- Resultado (Enviado, Fallido, Reintento)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (AlertId) REFERENCES Alerts(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- ============================================================
-- 14. Tabla: UserCompanies
-- Define las empresas a las que un usuario tiene acceso.
-- Ejemplo: un administrador de grupo puede ver todas las empresas.
-- ============================================================
CREATE TABLE UserCompanies (
    UserId UNIQUEIDENTIFIER NOT NULL,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    AccessType NVARCHAR(50) DEFAULT 'view',     -- Tipo de acceso (view, edit, manage)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    PRIMARY KEY (UserId, CompanyId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
);

-- ============================================================
-- 15. Tabla: UserGroups
-- Define los grupos a los que un usuario tiene acceso.
-- ============================================================
CREATE TABLE UserGroups (
    UserId UNIQUEIDENTIFIER NOT NULL,
    GroupId UNIQUEIDENTIFIER NOT NULL,
    AccessType NVARCHAR(50) DEFAULT 'view',     -- Tipo de acceso
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    PRIMARY KEY (UserId, GroupId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);

-- ============================================================
-- 16. Tabla: Logs (Auditoría)
-- Registra las acciones ejecutadas por los usuarios del sistema.
-- Compatible con eventos de .NET Core (EF SaveChanges, etc.)
-- ============================================================
CREATE TABLE Logs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL,
    Action NVARCHAR(100) NOT NULL,          -- Acción (CREATE, UPDATE, DELETE, LOGIN, etc.)
    TableName NVARCHAR(100) NOT NULL,       -- Tabla afectada
    RecordId UNIQUEIDENTIFIER NULL,         -- ID del registro afectado
    OldValues NVARCHAR(MAX) NULL,           -- JSON con valores previos
    NewValues NVARCHAR(MAX) NULL,           -- JSON con valores nuevos
    Timestamp DATETIME DEFAULT GETDATE(),   -- Fecha/hora del evento
    IpAddress NVARCHAR(50) NULL,            -- IP del usuario que ejecutó la acción
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO
