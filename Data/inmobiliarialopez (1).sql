-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 27-03-2025 a las 04:24:25
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliarialopez`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `IdContrato` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `MontoMensual` decimal(10,2) NOT NULL,
  `FechaTerminacion` date DEFAULT NULL,
  `Multa` decimal(10,2) DEFAULT NULL,
  `IdInmueble` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `CreadoPor` int(11) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `TerminadoPor` int(11) DEFAULT NULL,
  `FechaTerminacionRegistro` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`IdContrato`, `FechaInicio`, `FechaFin`, `MontoMensual`, `FechaTerminacion`, `Multa`, `IdInmueble`, `IdInquilino`, `CreadoPor`, `FechaCreacion`, `TerminadoPor`, `FechaTerminacionRegistro`) VALUES
(40, '2023-09-01', '2027-08-31', 30000.00, NULL, NULL, 1, 1, 1, '2025-03-25 20:15:47', NULL, NULL),
(41, '2023-10-01', '2027-09-30', 28000.00, NULL, NULL, 5, 2, 2, '2025-03-25 20:15:47', NULL, NULL),
(42, '2023-11-01', '2027-10-31', 38000.00, NULL, NULL, 8, 1, 1, '2025-03-25 20:15:47', NULL, NULL),
(43, '2023-12-01', '2027-11-30', 33000.00, NULL, NULL, 11, 2, 2, '2025-03-25 20:15:47', NULL, NULL),
(44, '2024-01-01', '2027-12-31', 36000.00, NULL, NULL, 14, 1, 1, '2025-03-25 20:15:47', NULL, NULL),
(45, '2020-01-01', '2020-12-31', 40000.00, NULL, NULL, 2, 2, 1, '2025-03-25 20:15:59', NULL, NULL),
(46, '2021-03-01', '2022-02-28', 50000.00, NULL, NULL, 3, 1, 2, '2025-03-25 20:15:59', NULL, NULL),
(47, '2019-06-01', '2020-05-31', 32000.00, NULL, NULL, 7, 2, 1, '2025-03-25 20:15:59', NULL, NULL),
(48, '2022-07-01', '2023-06-30', 42000.00, NULL, NULL, 10, 1, 2, '2025-03-25 20:15:59', NULL, NULL),
(49, '2018-09-01', '2019-08-31', 36000.00, NULL, NULL, 14, 2, 1, '2025-03-25 20:15:59', NULL, NULL),
(52, '2025-01-01', '2025-04-15', 40000.00, NULL, NULL, 2, 1, 1, '2025-03-25 21:05:55', NULL, NULL),
(53, '2025-02-01', '2025-04-30', 50000.00, NULL, NULL, 3, 2, 1, '2025-03-25 21:05:55', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `IdInmueble` int(11) NOT NULL,
  `Direccion` varchar(200) NOT NULL,
  `Uso` enum('Comercial','Residencial') NOT NULL,
  `IdTipoInmueble` int(11) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Coordenadas` varchar(100) DEFAULT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Estado` enum('Disponible','No Disponible','Alquilado') NOT NULL DEFAULT 'Disponible',
  `IdPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`IdInmueble`, `Direccion`, `Uso`, `IdTipoInmueble`, `Ambientes`, `Coordenadas`, `Precio`, `Estado`, `IdPropietario`) VALUES
(1, 'Via Montenapoleone 12, Milano', 'Residencial', 4, 3, '45.4679, 9.1910', 30000.00, 'Alquilado', 1),
(2, 'Piazza del Duomo 5, Milano', 'Comercial', 1, 1, '45.4642, 9.1900', 40000.00, 'Disponible', 2),
(3, 'Corso Buenos Aires 45, Milano', 'Residencial', 3, 5, '45.4736, 9.2181', 50000.00, 'Disponible', 1),
(4, 'Via del Corso 23, Roma', 'Comercial', 1, 1, '41.8987, 12.4828', 35000.00, 'Disponible', 2),
(5, 'Piazza Navona 8, Roma', 'Residencial', 4, 2, '41.8991, 12.4731', 28000.00, 'Alquilado', 1),
(6, 'Via Veneto 15, Roma', 'Residencial', 3, 4, '41.9074, 12.4870', 45000.00, 'Disponible', 2),
(7, 'Piazza della Signoria 3, Firenze', 'Comercial', 1, 1, '43.7696, 11.2558', 32000.00, 'Disponible', 1),
(8, 'Via Tornabuoni 10, Firenze', 'Residencial', 4, 3, '43.7730, 11.2558', 38000.00, 'Alquilado', 2),
(9, 'Via dei Calzaiuoli 7, Firenze', 'Residencial', 3, 6, '43.7696, 11.2558', 60000.00, 'Disponible', 1),
(10, 'Piazza San Marco 1, Venezia', 'Comercial', 1, 1, '45.4343, 12.3380', 42000.00, 'Disponible', 1),
(11, 'Calle Larga XXII Marzo 5, Venezia', 'Residencial', 4, 2, '45.4330, 12.3380', 33000.00, 'Alquilado', 2),
(12, 'Fondamenta delle Zattere 12, Venezia', 'Residencial', 3, 4, '45.4270, 12.3270', 55000.00, 'Disponible', 1),
(13, 'Via Toledo 20, Napoli', 'Comercial', 1, 1, '40.8476, 14.2563', 31000.00, 'Disponible', 2),
(14, 'Piazza del Plebiscito 4, Napoli', 'Residencial', 4, 3, '40.8396, 14.2520', 36000.00, 'Alquilado', 1),
(15, 'Via Chiaia 18, Napoli', 'Residencial', 3, 5, '40.8450, 14.2520', 48000.00, 'Disponible', 2),
(16, 'Via Roma 10, Torino', 'Comercial', 1, 1, '45.0679, 7.6825', 30000.00, 'Disponible', 1),
(17, 'Piazza Castello 3, Torino', 'Residencial', 4, 2, '45.0703, 7.6869', 29000.00, 'Disponible', 2),
(18, 'Corso Vittorio Emanuele II 25, Torino', 'Residencial', 3, 4, '45.0679, 7.6825', 47000.00, 'Disponible', 1),
(19, 'Piazza Maggiore 5, Bologna', 'Comercial', 1, 1, '44.4949, 11.3426', 34000.00, 'Disponible', 2),
(20, 'Via Rizzoli 12, Bologna', 'Residencial', 4, 3, '44.4938, 11.3426', 37000.00, 'Disponible', 1),
(21, 'Via dell\'Indipendenza 18, Bologna', 'Residencial', 3, 5, '44.4949, 11.3426', 52000.00, 'Disponible', 2),
(22, 'Via Garibaldi 8, Genova', 'Comercial', 1, 1, '44.4082, 8.9338', 33000.00, 'Disponible', 1),
(23, 'Piazza De Ferrari 4, Genova', 'Residencial', 4, 2, '44.4073, 8.9338', 30000.00, 'Disponible', 2),
(24, 'Via XX Settembre 10, Genova', 'Residencial', 3, 4, '44.4082, 8.9338', 49000.00, 'Disponible', 1),
(25, 'Piazza delle Erbe 3, Verona', 'Comercial', 1, 1, '45.4384, 10.9919', 32000.00, 'Disponible', 2),
(26, 'Via Mazzini 7, Verona', 'Residencial', 4, 3, '45.4390, 10.9919', 35000.00, 'Disponible', 1),
(27, 'Corso Porta Nuova 12, Verona', 'Residencial', 3, 5, '45.4384, 10.9919', 50000.00, 'Disponible', 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `IdInquilino` int(11) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`IdInquilino`, `DNI`, `Apellido`, `Nombre`, `Telefono`, `Email`) VALUES
(1, '25567890', 'Giordano', 'Roberto', '091678901', 'roberto.giordano@example.com'),
(2, '36678901', 'Rizzo', 'Claudio', '011789012', 'claudio.rizzo@example.com'),
(3, '45789012', 'Lombardi', 'Emanuele', '040890123', 'emanuele.lombardi@example.com'),
(4, '26890123', 'Moretti', 'Marco', '080901234', 'marco.moretti@example.com'),
(5, '37901234', 'Barbieri', 'Paolo', '070012345', 'paolo.barbieri@example.com'),
(6, '46012345', 'Fontana', 'Alberto', '032123456', 'alberto.fontana@example.com'),
(7, '27123456', 'Santoro', 'Nicola', '010234567', 'nicola.santoro@example.com'),
(8, '38234567', 'Mariani', 'Enrico', '051345678', 'enrico.mariani@example.com'),
(9, '47345678', 'Rinaldi', 'Federico', '081456789', 'federico.rinaldi@example.com'),
(10, '28456789', 'Caruso', 'Gabriele', '055567890', 'gabriele.caruso@example.com'),
(11, '39567890', 'Ferrara', 'Massimo', '091678901', 'massimo.ferrara@example.com'),
(12, '48678901', 'Martini', 'Diego', '011789012', 'diego.martini@example.com'),
(13, '29789012', 'Leone', 'Valerio', '040890123', 'valerio.leone@example.com'),
(14, '49890123', 'Longo', 'Samuele', '080901234', 'samuele.longo@example.com'),
(15, '30901234', 'Galli', 'Tommaso', '070012345', 'tommaso.galli@example.com'),
(16, '40012345', 'Benedetti', 'Filippo', '032123456', 'filippo.benedetti@example.com'),
(17, '31123456', 'Rossetti', 'Jacopo', '010234567', 'jacopo.rossetti@example.com'),
(18, '41234567', 'Marchesi', 'Edoardo', '051345678', 'edoardo.marchesi@example.com'),
(19, '32345678', 'Pellegrini', 'Leonardo', '081456789', 'leonardo.pellegrini@example.com'),
(20, '42456789', 'Palumbo', 'Cristiano', '055567890', 'cristiano.palumbo@example.com'),
(21, '33567890', 'Vitale', 'Daniele', '091678901', 'daniele.vitale@example.com'),
(22, '43678901', 'Silvestri', 'Luigi', '011789012', 'luigi.silvestri@example.com'),
(23, '34789012', 'Testa', 'Riccardo', '040890123', 'riccardo.testa@example.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `IdPago` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Detalle` varchar(100) NOT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `Estado` enum('Activo','Anulado') NOT NULL DEFAULT 'Activo',
  `AnuladoPor` int(11) DEFAULT NULL,
  `FechaAnulacion` datetime DEFAULT NULL,
  `IdContrato` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`IdPago`, `NumeroPago`, `FechaPago`, `Detalle`, `Importe`, `Estado`, `AnuladoPor`, `FechaAnulacion`, `IdContrato`) VALUES
(1, 19, '2025-03-01', 'Mes 19', 30000.00, 'Activo', NULL, NULL, 40),
(2, 18, '2025-03-01', 'Mes 18', 28000.00, 'Activo', NULL, NULL, 41),
(3, 17, '2025-03-01', 'Mes 17', 38000.00, 'Activo', NULL, NULL, 42),
(4, 16, '2025-03-01', 'Mes 16', 33000.00, 'Activo', NULL, NULL, 43),
(5, 15, '2025-03-01', 'Mes 15', 36000.00, 'Activo', NULL, NULL, 44),
(8, 1, '2020-01-01', 'Mes 1', 40000.00, 'Activo', NULL, NULL, 45),
(9, 1, '2021-03-01', 'Mes 1', 50000.00, 'Activo', NULL, NULL, 46),
(10, 1, '2019-06-01', 'Mes 1', 32000.00, 'Activo', NULL, NULL, 47),
(11, 1, '2022-07-01', 'Mes 1', 42000.00, 'Activo', NULL, NULL, 48),
(12, 1, '2018-09-01', 'Mes 1', 36000.00, 'Activo', NULL, NULL, 49),
(13, 2, '2020-02-01', 'Mes 2', 40000.00, 'Activo', NULL, NULL, 45),
(14, 2, '2021-04-01', 'Mes 2', 50000.00, 'Activo', NULL, NULL, 46),
(15, 2, '2019-07-01', 'Mes 2', 32000.00, 'Activo', NULL, NULL, 47),
(16, 2, '2022-08-01', 'Mes 2', 42000.00, 'Activo', NULL, NULL, 48),
(17, 2, '2018-10-01', 'Mes 2', 36000.00, 'Activo', NULL, NULL, 49),
(18, 3, '2020-03-01', 'Mes 3', 40000.00, 'Activo', NULL, NULL, 45),
(19, 3, '2021-05-01', 'Mes 3', 50000.00, 'Activo', NULL, NULL, 46),
(20, 3, '2019-08-01', 'Mes 3', 32000.00, 'Activo', NULL, NULL, 47),
(21, 3, '2022-09-01', 'Mes 3', 42000.00, 'Activo', NULL, NULL, 48),
(22, 3, '2018-11-01', 'Mes 3', 36000.00, 'Activo', NULL, NULL, 49),
(23, 4, '2020-04-01', 'Mes 4', 40000.00, 'Activo', NULL, NULL, 45),
(24, 4, '2021-06-01', 'Mes 4', 50000.00, 'Activo', NULL, NULL, 46),
(25, 4, '2019-09-01', 'Mes 4', 32000.00, 'Activo', NULL, NULL, 47),
(26, 4, '2022-10-01', 'Mes 4', 42000.00, 'Activo', NULL, NULL, 48),
(27, 4, '2018-12-01', 'Mes 4', 36000.00, 'Activo', NULL, NULL, 49),
(28, 5, '2020-05-01', 'Mes 5', 40000.00, 'Activo', NULL, NULL, 45),
(29, 5, '2021-07-01', 'Mes 5', 50000.00, 'Activo', NULL, NULL, 46),
(30, 5, '2019-10-01', 'Mes 5', 32000.00, 'Activo', NULL, NULL, 47),
(31, 5, '2022-11-01', 'Mes 5', 42000.00, 'Activo', NULL, NULL, 48),
(32, 5, '2019-01-01', 'Mes 5', 36000.00, 'Activo', NULL, NULL, 49),
(33, 6, '2020-06-01', 'Mes 6', 40000.00, 'Activo', NULL, NULL, 45),
(34, 6, '2021-08-01', 'Mes 6', 50000.00, 'Activo', NULL, NULL, 46),
(35, 6, '2019-11-01', 'Mes 6', 32000.00, 'Activo', NULL, NULL, 47),
(36, 6, '2022-12-01', 'Mes 6', 42000.00, 'Activo', NULL, NULL, 48),
(37, 6, '2019-02-01', 'Mes 6', 36000.00, 'Activo', NULL, NULL, 49),
(38, 7, '2020-07-01', 'Mes 7', 40000.00, 'Activo', NULL, NULL, 45),
(39, 7, '2021-09-01', 'Mes 7', 50000.00, 'Activo', NULL, NULL, 46),
(40, 7, '2019-12-01', 'Mes 7', 32000.00, 'Activo', NULL, NULL, 47),
(41, 7, '2023-01-01', 'Mes 7', 42000.00, 'Activo', NULL, NULL, 48),
(42, 7, '2019-03-01', 'Mes 7', 36000.00, 'Activo', NULL, NULL, 49),
(43, 8, '2020-08-01', 'Mes 8', 40000.00, 'Activo', NULL, NULL, 45),
(44, 8, '2021-10-01', 'Mes 8', 50000.00, 'Activo', NULL, NULL, 46),
(45, 8, '2020-01-01', 'Mes 8', 32000.00, 'Activo', NULL, NULL, 47),
(46, 8, '2023-02-01', 'Mes 8', 42000.00, 'Activo', NULL, NULL, 48),
(47, 8, '2019-04-01', 'Mes 8', 36000.00, 'Activo', NULL, NULL, 49),
(48, 9, '2020-09-01', 'Mes 9', 40000.00, 'Activo', NULL, NULL, 45),
(49, 9, '2021-11-01', 'Mes 9', 50000.00, 'Activo', NULL, NULL, 46),
(50, 9, '2020-02-01', 'Mes 9', 32000.00, 'Activo', NULL, NULL, 47),
(51, 9, '2023-03-01', 'Mes 9', 42000.00, 'Activo', NULL, NULL, 48),
(52, 9, '2019-05-01', 'Mes 9', 36000.00, 'Activo', NULL, NULL, 49),
(53, 10, '2020-10-01', 'Mes 10', 40000.00, 'Activo', NULL, NULL, 45),
(54, 10, '2021-12-01', 'Mes 10', 50000.00, 'Activo', NULL, NULL, 46),
(55, 10, '2020-03-01', 'Mes 10', 32000.00, 'Activo', NULL, NULL, 47),
(56, 10, '2023-04-01', 'Mes 10', 42000.00, 'Activo', NULL, NULL, 48),
(57, 10, '2019-06-01', 'Mes 10', 36000.00, 'Activo', NULL, NULL, 49),
(58, 11, '2020-11-01', 'Mes 11', 40000.00, 'Activo', NULL, NULL, 45),
(59, 11, '2022-01-01', 'Mes 11', 50000.00, 'Activo', NULL, NULL, 46),
(60, 11, '2020-04-01', 'Mes 11', 32000.00, 'Activo', NULL, NULL, 47),
(61, 11, '2023-05-01', 'Mes 11', 42000.00, 'Activo', NULL, NULL, 48),
(62, 11, '2019-07-01', 'Mes 11', 36000.00, 'Activo', NULL, NULL, 49),
(63, 12, '2020-12-01', 'Mes 12', 40000.00, 'Activo', NULL, NULL, 45),
(64, 12, '2022-02-01', 'Mes 12', 50000.00, 'Activo', NULL, NULL, 46),
(65, 12, '2020-05-01', 'Mes 12', 32000.00, 'Activo', NULL, NULL, 47),
(66, 12, '2023-06-01', 'Mes 12', 42000.00, 'Activo', NULL, NULL, 48),
(67, 12, '2019-08-01', 'Mes 12', 36000.00, 'Activo', NULL, NULL, 49);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `IdPropietario` int(11) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`IdPropietario`, `DNI`, `Apellido`, `Nombre`, `Telefono`, `Email`) VALUES
(1, '20345678', 'Rossi', 'Mario', '021234567', 'mario.rossi@example.com'),
(2, '31234567', 'Russo', 'Luca', '062345678', 'luca.russo@example.com'),
(3, '40987654', 'Ferrari', 'Giovanni', '041345678', 'giovanni.ferrari@example.com'),
(4, '21345678', 'Esposito', 'Antonio', '081456789', 'antonio.esposito@example.com'),
(5, '32456789', 'Bianchi', 'Francesco', '055567890', 'francesco.bianchi@example.com'),
(6, '41567890', 'Romano', 'Giuseppe', '091678901', 'giuseppe.romano@example.com'),
(7, '22678901', 'Colombo', 'Andrea', '011789012', 'andrea.colombo@example.com'),
(8, '33789012', 'Ricci', 'Matteo', '040890123', 'matteo.ricci@example.com'),
(9, '42890123', 'Marino', 'Lorenzo', '080901234', 'lorenzo.marino@example.com'),
(10, '23901234', 'Greco', 'Vincenzo', '070012345', 'vincenzo.greco@example.com'),
(11, '34012345', 'Bruno', 'Stefano', '032123456', 'stefano.bruno@example.com'),
(12, '43123456', 'Gallo', 'Alessandro', '010234567', 'alessandro.gallo@example.com'),
(13, '24234567', 'Conti', 'Simone', '051345678', 'simone.conti@example.com'),
(14, '35345678', 'Mancini', 'Davide', '081456789', 'davide.mancini@example.com'),
(15, '44456789', 'Costa', 'Fabio', '055567890', 'fabio.costa@example.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `IdTipoInmueble` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`IdTipoInmueble`, `Nombre`) VALUES
(1, 'Casa'),
(2, 'Departamento'),
(3, 'Local'),
(4, 'Depósito'),
(5, 'Local'),
(6, 'Depósito'),
(7, 'Casa'),
(8, 'Departamento'),
(9, 'Oficina'),
(10, 'Galpón');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `IdUsuario` int(11) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Contrasena` varchar(100) NOT NULL,
  `Rol` enum('Administrador','Empleado') NOT NULL,
  `Avatar` varchar(200) DEFAULT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`IdUsuario`, `Email`, `Contrasena`, `Rol`, `Avatar`, `FechaCreacion`) VALUES
(1, 'admin1@example.com', 'admin123', 'Administrador', 'https://example.com/avatars/admin1.jpg', '2025-03-25 20:01:08'),
(2, 'empleado1@example.com', 'empleado123', 'Empleado', 'https://example.com/avatars/empleado1.jpg', '2025-03-25 20:01:08'),
(3, 'empleado2@example.com', 'empleado456', 'Empleado', 'https://example.com/avatars/empleado2.jpg', '2025-03-25 20:01:08'),
(4, 'admin2@example.com', 'admin456', 'Administrador', 'https://example.com/avatars/admin2.jpg', '2025-03-25 20:01:08'),
(5, 'empleado3@example.com', 'empleado789', 'Empleado', NULL, '2025-03-25 20:01:08');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `IdInmueble` (`IdInmueble`),
  ADD KEY `IdInquilino` (`IdInquilino`),
  ADD KEY `CreadoPor` (`CreadoPor`),
  ADD KEY `TerminadoPor` (`TerminadoPor`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `IdTipoInmueble` (`IdTipoInmueble`),
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`IdInquilino`),
  ADD UNIQUE KEY `DNI` (`DNI`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `IdContrato` (`IdContrato`),
  ADD KEY `AnuladoPor` (`AnuladoPor`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`IdPropietario`),
  ADD UNIQUE KEY `DNI` (`DNI`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`IdTipoInmueble`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`IdUsuario`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=68;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `IdTipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`IdInmueble`) REFERENCES `inmueble` (`IdInmueble`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilino` (`IdInquilino`),
  ADD CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`CreadoPor`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `contrato_ibfk_4` FOREIGN KEY (`TerminadoPor`) REFERENCES `usuario` (`IdUsuario`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`IdTipoInmueble`) REFERENCES `tipoinmueble` (`IdTipoInmueble`),
  ADD CONSTRAINT `inmueble_ibfk_2` FOREIGN KEY (`IdPropietario`) REFERENCES `propietario` (`IdPropietario`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`IdContrato`),
  ADD CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`AnuladoPor`) REFERENCES `usuario` (`IdUsuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
