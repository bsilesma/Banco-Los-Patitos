CREATE DATABASE BancoLosPatitosDB;
GO

USE BancoLosPatitosDB;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Comercios
CREATE TABLE dbo.Comercios
(
    IdComercio          INT           IDENTITY(1,1) NOT NULL,
    Identificacion      VARCHAR(30)   NOT NULL,
    TipoIdentificacion  INT           NOT NULL,  -- 1 = Física, 2 = Jurídica
    Nombre              VARCHAR(200)  NOT NULL,
    TipoDeComercio      INT           NOT NULL,  -- 1 = Restaurantes, 2 = Supermercados, 3 = Ferreterías, 4 = Otros
    Telefono            VARCHAR(20)   NOT NULL,
    CorreoElectronico   VARCHAR(200)  NOT NULL,
    Direccion           VARCHAR(500)  NOT NULL,
    FechaDeRegistro     DATETIME      NOT NULL,
    FechaDeModificacion DATETIME      NULL,
    Estado              BIT           NOT NULL,  -- 1 = Activo, 0 = Inactivo

    CONSTRAINT PK_Comercios
        PRIMARY KEY CLUSTERED (IdComercio ASC),

    -- Regla: no pueden existir dos comercios con la misma identificación.
    CONSTRAINT UQ_Comercios_Identificacion
        UNIQUE (Identificacion),

    CONSTRAINT CK_Comercios_TipoIdentificacion
        CHECK (TipoIdentificacion IN (1, 2)),

    CONSTRAINT CK_Comercios_TipoDeComercio
        CHECK (TipoDeComercio BETWEEN 1 AND 4)
);
GO

-- 2. Cajas
CREATE TABLE dbo.Cajas
(
    IdCaja              INT           IDENTITY(1,1) NOT NULL,
    IdComercio          INT           NOT NULL,
    Nombre              VARCHAR(100)  NOT NULL,
    Descripcion         VARCHAR(150)  NOT NULL,
    TelefonoSINPE       VARCHAR(10)   NOT NULL,
    FechaDeRegistro     DATETIME      NOT NULL,
    FechaDeModificacion DATETIME      NULL,
    Estado              BIT           NOT NULL,  -- 1 = Activo, 0 = Inactivo

    CONSTRAINT PK_Cajas
        PRIMARY KEY CLUSTERED (IdCaja ASC),

    CONSTRAINT FK_Cajas_Comercios
        FOREIGN KEY (IdComercio)
        REFERENCES dbo.Comercios (IdComercio),

    -- Regla: no pueden existir dos cajas con el mismo nombre en un comercio.
    CONSTRAINT UQ_Cajas_IdComercio_Nombre
        UNIQUE (IdComercio, Nombre)
);
GO

-- Regla: no pueden existir dos cajas con el mismo teléfono ACTIVO en toda la tabla.
CREATE UNIQUE NONCLUSTERED INDEX UX_Cajas_TelefonoSINPE_Activas
    ON dbo.Cajas (TelefonoSINPE)
    WHERE Estado = 1;
GO

-- 3. Sinpe

CREATE TABLE dbo.Sinpe
(
    IdSinpe              INT            IDENTITY(1,1) NOT NULL,
    TelefonoOrigen       VARCHAR(10)    NOT NULL,
    NombreOrigen         VARCHAR(200)   NOT NULL,
    TelefonoDestinatario VARCHAR(10)    NOT NULL,
    NombreDestinatario   VARCHAR(200)   NOT NULL,
    Monto                DECIMAL(18,2)  NOT NULL,
    FechaDeRegistro      DATETIME       NOT NULL,
    Descripcion          VARCHAR(50)    NULL,
    Estado               BIT            NOT NULL,  -- 1 = Sincronizado, 0 = No sincronizado

    CONSTRAINT PK_Sinpe
        PRIMARY KEY CLUSTERED (IdSinpe ASC),

    -- Un pago de 0 o negativo no tiene sentido.
    CONSTRAINT CK_Sinpe_Monto
        CHECK (Monto > 0)

);
GO

-- 4. BITACORA_EVENTOS
CREATE TABLE dbo.BITACORA_EVENTOS
(
    IdEvento            INT           IDENTITY(1,1) NOT NULL,
    TablaDeEvento       VARCHAR(20)   NOT NULL,
    TipoDeEvento        VARCHAR(20)   NOT NULL,
    FechaDeEvento       DATETIME      NOT NULL,
    DescripcionDeEvento VARCHAR(MAX)  NOT NULL,
    StackTrace          VARCHAR(MAX)  NOT NULL,
    DatosAnteriores     VARCHAR(MAX)  NULL,  
    DatosPosteriores    VARCHAR(MAX)  NULL,  

    CONSTRAINT PK_BITACORA_EVENTOS
        PRIMARY KEY CLUSTERED (IdEvento ASC),

    CONSTRAINT CK_BITACORA_EVENTOS_TipoDeEvento
        CHECK (TipoDeEvento IN ('Registrar', 'Editar', 'Eliminar', 'Error'))
);
GO

-- 5. Configuraciones
CREATE TABLE dbo.Configuraciones
(
    IdConfiguracion     INT       IDENTITY(1,1) NOT NULL,
    IdComercio          INT       NOT NULL,
    TipoConfiguracion   INT       NOT NULL,  -- 1 = Plataforma, 2 = Externa, 3 = Ambas
    Comision            INT       NOT NULL,  -- Porcentaje: 20 equivale a 20 %
    FechaDeRegistro     DATETIME  NOT NULL,
    FechaDeModificacion DATETIME  NULL,
    Estado              BIT       NOT NULL,  -- 1 = Activo, 0 = Inactivo

    CONSTRAINT PK_Configuraciones
        PRIMARY KEY CLUSTERED (IdConfiguracion ASC),

    CONSTRAINT FK_Configuraciones_Comercios
        FOREIGN KEY (IdComercio)
        REFERENCES dbo.Comercios (IdComercio),

    -- Regla: un comercio solo puede tener una configuración.
    CONSTRAINT UQ_Configuraciones_IdComercio
        UNIQUE (IdComercio),

    CONSTRAINT CK_Configuraciones_TipoConfiguracion
        CHECK (TipoConfiguracion BETWEEN 1 AND 3),

    CONSTRAINT CK_Configuraciones_Comision
        CHECK (Comision BETWEEN 1 AND 100)
);
GO

-- 6. Usuarios
CREATE TABLE dbo.Usuarios
(
    IdUsuario           INT              IDENTITY(1,1) NOT NULL,
    IdComercio          INT              NOT NULL,
    IdNetUser           UNIQUEIDENTIFIER NULL,  -- Se llena cuando se integre .NET Identity
    Nombres             VARCHAR(100)     NOT NULL,
    PrimerApellido      VARCHAR(100)     NOT NULL,
    SegundoApellido     VARCHAR(100)     NOT NULL,
    Identificacion      VARCHAR(10)      NOT NULL,
    CorreoElectronico   VARCHAR(200)     NOT NULL,
    FechaDeRegistro     DATETIME         NOT NULL,
    FechaDeModificacion DATETIME         NULL,
    Estado              BIT              NOT NULL,  -- 1 = Activo, 0 = Inactivo

    CONSTRAINT PK_Usuarios
        PRIMARY KEY CLUSTERED (IdUsuario ASC),

    CONSTRAINT FK_Usuarios_Comercios
        FOREIGN KEY (IdComercio)
        REFERENCES dbo.Comercios (IdComercio),

    -- Regla: no pueden existir dos usuarios con la misma identificación.
    CONSTRAINT UQ_Usuarios_Identificacion
        UNIQUE (Identificacion)
);
GO

-- 7. ReportesMensuales
CREATE TABLE dbo.ReportesMensuales
(
    IdReporte           INT            IDENTITY(1,1) NOT NULL,
    IdComercio          INT            NOT NULL,
    CantidadDeCajas     INT            NOT NULL,
    MontoTotalRecaudado DECIMAL(18,2)  NOT NULL,
    CantidadDeSINPES    INT            NOT NULL,
    MontoTotalComision  DECIMAL(18,2)  NOT NULL,
    FechaDelReporte     DATETIME       NOT NULL,

    CONSTRAINT PK_ReportesMensuales
        PRIMARY KEY CLUSTERED (IdReporte ASC),

    CONSTRAINT FK_ReportesMensuales_Comercios
        FOREIGN KEY (IdComercio)
        REFERENCES dbo.Comercios (IdComercio)
);
GO
