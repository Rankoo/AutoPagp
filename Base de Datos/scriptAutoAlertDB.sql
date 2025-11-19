CREATE DATABASE AutoAlertDB;
GO

USE AutoAlertDB;
GO

-- ============================================================
--  Script de creación de base de datos: AutoAlertDB
--  Autor: Aswin Niño
--  Descripción: Estructura relacional para la gestión de alertas 
--  de servicios públicos con control de acceso, roles, permisos,
--  auditoría y configuración por grupos empresariales.
-- ============================================================


-- ============================================================
-- 1. Tabla: Groups
-- Contiene los grupos empresariales. 
-- Cada grupo puede tener múltiples empresas asociadas.
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
-- Contiene las empresas pertenecientes a un grupo.
-- Cada empresa puede tener múltiples tiendas o puntos de venta.
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
-- Contiene las tiendas o puntos de venta asociados a cada empresa.
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
-- Catálogo maestro de tipos de documento (CC, NIT, CE, etc.).
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
-- Define los diferentes roles del sistema (Administrador, Usuario, etc.).
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
-- Contiene los usuarios del sistema, asociados a roles y tipos de documento.
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
    Position NVARCHAR(100) NULL,
    IsActive BIT DEFAULT 1,
    ChangePassword BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    FOREIGN KEY (DocumentTypeId) REFERENCES DocumentTypes(Id)
);

-- ============================================================
-- 7. Tabla: Modules
-- Define los módulos principales del sistema (por ejemplo: Administración, Reportes).
-- ============================================================
CREATE TABLE Modules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- ============================================================
-- 8. Tabla: SubModules
-- Define los submódulos o funcionalidades dentro de cada módulo.
-- ============================================================
CREATE TABLE SubModules (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ModuleId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    FOREIGN KEY (ModuleId) REFERENCES Modules(Id)
);

-- ============================================================
-- 9. Tabla: RoleSubModules
-- Define los permisos por rol sobre cada submódulo.
-- Ejemplo: El rol “Administrador” tiene acceso al submódulo “Configuraciones”.
-- ============================================================
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

-- ============================================================
-- 10. Tabla: UserSubModules
-- Define permisos personalizados de usuario que sobrescriben los permisos del rol.
-- Ejemplo: Un usuario estándar puede obtener acceso temporal a un submódulo específico.
-- ============================================================
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
-- Contiene los servicios públicos asociados a las tiendas (agua, luz, gas, etc.).
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
    ServiceId UNIQUEIDENTIFIER NOT NULL,        -- Servicio asociado a la alerta
    ScheduledAt DATETIME NOT NULL,              -- Fecha y hora programada para el envío
    Channel NVARCHAR(50),                       -- Canal de envío (WhatsApp, Email, SMS)
    Status NVARCHAR(50),                        -- Estado (Programada, Enviada, Fallida)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (ServiceId) REFERENCES Services(Id)
);

-- ============================================================
-- 13. Tabla: Notifications
-- Guarda el historial de alertas efectivamente enviadas a los usuarios.
-- ============================================================
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    AlertId UNIQUEIDENTIFIER NOT NULL,          -- Alerta enviada
    UserId UNIQUEIDENTIFIER NOT NULL,           -- Usuario destinatario
    SentAt DATETIME,                            -- Fecha real de envío
    Result NVARCHAR(100),                       -- Resultado (Enviado, Fallido, Reintento)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (AlertId) REFERENCES Alerts(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- ============================================================
-- 14. Tabla: UserCompanies
-- Define las empresas a las que un usuario tiene acceso.
-- Ejemplo: un usuario con rol de “Administrador de Grupo” puede acceder a todas las empresas del grupo.
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
    AccessType NVARCHAR(50) DEFAULT 'view',     -- Tipo de acceso (view, edit, manage)
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,
    PRIMARY KEY (UserId, GroupId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);

-- ============================================================
-- 16. Tabla: Logs (Auditoría)
-- Registra las acciones ejecutadas por los usuarios del sistema.
-- Compatible con eventos generados desde .NET Core (SaveChanges, etc.).
-- ============================================================
CREATE TABLE Logs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL,
    Action NVARCHAR(100) NOT NULL,          -- Acción (CREATE, UPDATE, DELETE, LOGIN, etc.)
    TableName NVARCHAR(100) NOT NULL,       -- Tabla afectada
    RecordId UNIQUEIDENTIFIER NULL,         -- ID del registro afectado
    OldValues NVARCHAR(MAX) NULL,           -- JSON con valores previos
    NewValues NVARCHAR(MAX) NULL,           -- JSON con valores nuevos
    Timestamp DATETIME DEFAULT GETDATE(),   -- Fecha y hora del evento
    IpAddress NVARCHAR(50) NULL,            -- Dirección IP del usuario
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO


insert into DocumentTypes (Name, Abbreviation) 
values  ('Cédula de Ciudadanía','CC'),
        ('Cédula de Extranjería', 'CE'),
        ('Tarjeta de Identidad', 'TI');
GO

insert into Roles (Name) values ('Admin'),('Supervisor'),('User');
GO
