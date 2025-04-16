-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 15-04-2025 a las 23:20:22
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
  `IdUsuarioCrea` int(11) DEFAULT NULL,
  `FechaCreacion` datetime NOT NULL,
  `IdUsuarioAnula` int(11) DEFAULT NULL,
  `FechaRescision` date DEFAULT NULL,
  `FechaUsuarioAnula` datetime DEFAULT NULL,
  `IdInmueble` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `MontoMensual` decimal(10,2) NOT NULL,
  `Multa` decimal(10,2) DEFAULT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1,
  `EstadoContrato` enum('Vigente','Finalizado','Anulado','PendienteAnulacion') DEFAULT 'Vigente',
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`IdContrato`, `IdUsuarioCrea`, `FechaCreacion`, `IdUsuarioAnula`, `FechaRescision`, `FechaUsuarioAnula`, `IdInmueble`, `IdInquilino`, `FechaInicio`, `FechaFin`, `MontoMensual`, `Multa`, `Activo`, `EstadoContrato`, `Observaciones`) VALUES
(40, 1, '0001-01-01 00:00:00', 2, '2025-05-01', '2025-04-15 15:25:54', 1, 1, '2023-09-01', '2027-11-27', 55500555.00, NULL, 1, 'PendienteAnulacion', ''),
(41, 2, '2023-09-30 00:00:00', 2, '2025-05-31', '2025-04-15 12:34:15', 5, 2, '2023-10-16', '2025-09-30', 25000.00, NULL, 1, 'PendienteAnulacion', NULL),
(42, 1, '2023-10-29 00:00:00', 5, '2025-06-21', '2025-04-15 13:33:36', 8, 1, '2023-11-01', '2027-10-31', 38000.00, NULL, 1, 'PendienteAnulacion', NULL),
(43, 2, '2023-11-28 00:00:00', 2, '2025-04-17', '2025-04-15 14:14:44', 11, 2, '2023-12-01', '2027-11-30', 33000.00, NULL, 1, 'PendienteAnulacion', NULL),
(44, 1, '2023-12-27 00:00:00', 5, '2025-06-20', '2025-04-15 14:23:10', 14, 1, '2024-01-01', '2027-12-31', 36000.00, NULL, 1, 'PendienteAnulacion', 'probando de nuevo'),
(45, 1, '2019-12-29 00:00:00', 2, '2025-06-22', '2025-04-15 17:25:21', 2, 2, '2020-01-01', '2020-12-31', 40000.00, NULL, 1, 'PendienteAnulacion', 'El inquilino necesita dejar la casa porque tiene problemas económicos '),
(46, 2, '2021-02-28 00:00:00', NULL, NULL, NULL, 3, 1, '2021-03-01', '2022-02-28', 50000.00, NULL, 1, 'Vigente', NULL),
(47, 1, '2019-05-26 00:00:00', NULL, NULL, NULL, 7, 2, '2019-06-01', '2020-05-31', 32000.00, NULL, 1, 'Vigente', NULL),
(48, 2, '2022-06-24 00:00:00', NULL, NULL, NULL, 10, 1, '2022-07-01', '2023-06-30', 42000.00, NULL, 1, 'Vigente', NULL),
(49, 1, '2018-08-25 00:00:00', NULL, NULL, NULL, 14, 2, '2018-09-01', '2019-08-31', 36000.00, NULL, 1, 'Vigente', NULL),
(52, 1, '2024-12-31 00:00:00', NULL, NULL, NULL, 2, 1, '2025-01-01', '2025-04-15', 40000.00, NULL, 1, 'Vigente', NULL),
(53, 1, '2025-01-27 00:00:00', NULL, NULL, NULL, 3, 2, '2025-02-01', '2025-04-30', 50000.00, NULL, 1, 'Vigente', NULL),
(60, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 17, 2, '2025-04-12', '2026-04-12', 155000.00, NULL, 1, 'Vigente', NULL),
(61, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 9, 18, '2025-04-12', '2026-04-12', 152400.00, NULL, 1, 'Vigente', NULL),
(62, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 9, 23, '2026-04-13', '2027-04-11', 444000.00, NULL, 1, 'Vigente', NULL),
(63, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 17, 10, '2025-04-14', '2026-04-14', 555555.00, NULL, 1, 'Vigente', NULL);

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
  `Superficie` int(11) NOT NULL DEFAULT 0,
  `Latitud` decimal(10,6) DEFAULT NULL,
  `Longitud` decimal(10,6) DEFAULT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Estado` enum('Disponible','No Disponible','Alquilado') NOT NULL DEFAULT 'Disponible',
  `IdPropietario` int(11) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`IdInmueble`, `Direccion`, `Uso`, `IdTipoInmueble`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `Precio`, `Estado`, `IdPropietario`, `Activo`) VALUES
(1, 'Via Montenapoleone 11, Milano', 'Residencial', 4, 3, 180, 45.467900, 9.191000, 37000.00, 'Alquilado', 1, 1),
(2, 'Piazza del Duomo 5, Milano', 'Comercial', 1, 1, 40, 45.464200, 9.190000, 40000.00, 'Disponible', 2, 1),
(3, 'Corso Buenos Aires 45, Milano', 'Residencial', 3, 5, 250, 45.473600, 9.218100, 50000.00, 'Disponible', 3, 1),
(4, 'Via del Corso 23, Roma', 'Comercial', 1, 1, 40, 41.898700, 12.482800, 35000.00, 'Disponible', 4, 1),
(5, 'Piazza Navona 8, Roma', 'Residencial', 4, 2, 120, 41.899100, 12.473100, 28000.00, 'Alquilado', 5, 1),
(6, 'Via Veneto 15, Roma', 'Residencial', 3, 4, 200, 41.907400, 12.487000, 45000.00, 'Disponible', 6, 1),
(7, 'Piazza della Signoria 3, Firenze', 'Comercial', 1, 1, 40, 43.769600, 11.255800, 32000.00, 'Disponible', 7, 1),
(8, 'Via Tornabuoni 10, Firenze', 'Residencial', 4, 3, 180, 43.773000, 11.255800, 38000.00, 'Alquilado', 8, 1),
(9, 'Via dei Calzaiuoli 7, Firenze', 'Residencial', 3, 6, 300, 43.769600, 11.255800, 60000.00, 'Disponible', 9, 1),
(10, 'Piazza San Marco 1, Venezia', 'Comercial', 1, 1, 40, 45.434300, 12.338000, 42000.00, 'Disponible', 10, 1),
(11, 'Calle Larga XXII Marzo 5, Venezia', 'Residencial', 4, 2, 120, 45.433000, 12.338000, 33000.00, 'Alquilado', 11, 1),
(12, 'Fondamenta delle Zattere 12, Venezia', 'Residencial', 3, 4, 200, 45.427000, 12.327000, 55000.00, 'Disponible', 12, 1),
(13, 'Via Toledo 20, Napoli', 'Comercial', 1, 1, 40, 40.847600, 14.256300, 31000.00, 'Disponible', 13, 1),
(14, 'Piazza del Plebiscito 4, Napoli', 'Residencial', 4, 3, 180, 40.839600, 14.252000, 36000.00, 'Alquilado', 14, 1),
(15, 'Via Chiaia 18, Napoli', 'Residencial', 3, 5, 250, 40.845000, 14.252000, 48000.00, 'Disponible', 15, 1),
(16, 'Via Roma 10, Torino', 'Comercial', 1, 1, 40, 45.067900, 7.682500, 30000.00, 'Disponible', 19, 1),
(17, 'Piazza Castello 3, Torino', 'Residencial', 4, 2, 120, 45.070300, 7.686900, 23000.00, 'Disponible', 1, 1),
(18, 'Corso Vittorio Emanuele II 25, Torino', 'Residencial', 3, 4, 200, 45.067900, 7.682500, 47000.00, 'Disponible', 2, 1),
(19, 'Piazza Maggiore 5, Bologna', 'Comercial', 1, 1, 40, 44.494900, 11.342600, 34000.00, 'Disponible', 3, 1),
(20, 'Via Rizzoli 12, Bologna', 'Residencial', 4, 3, 180, 44.493800, 11.342600, 37000.00, 'Disponible', 4, 1),
(21, 'Via dell\'Indipendenza 18, Bologna', 'Residencial', 3, 5, 250, 44.494900, 11.342600, 52000.00, 'Disponible', 5, 1),
(22, 'Via Garibaldi 8, Genova', 'Comercial', 1, 1, 40, 44.408200, 8.933800, 33000.00, 'Disponible', 6, 1),
(23, 'Piazza De Ferrari 4, Genova', 'Residencial', 4, 2, 120, 44.407300, 8.933800, 30000.00, 'Disponible', 7, 1),
(24, 'Via XX Settembre 10, Genova', 'Residencial', 3, 4, 200, 44.408200, 8.933800, 49000.00, 'Disponible', 8, 1),
(25, 'Piazza delle Erbe 3, Verona', 'Comercial', 1, 1, 40, 45.438400, 10.991900, 32000.00, 'Disponible', 9, 1),
(26, 'Via Mazzini 7, Verona', 'Residencial', 4, 3, 180, 45.439000, 10.991900, 35000.00, 'Disponible', 10, 1),
(27, 'Corso Porta Nuova 12, Verona', 'Residencial', 3, 5, 250, 45.438400, 10.991900, 50000.00, 'Disponible', 11, 1);

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
  `Email` varchar(100) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`IdInquilino`, `DNI`, `Apellido`, `Nombre`, `Telefono`, `Email`, `Activo`) VALUES
(1, '25567890', 'Giordano', 'Roberto', '091678901', 'roberto.giordano@example.com', 1),
(2, '36678901', 'Rizzo', 'Claudia', '011789012', 'claudio.rizzo@example.com', 1),
(3, '45789012', 'Lombardi', 'Emanuele', '040890123', 'emanuele.lombardi@example.com', 1),
(4, '26890123', 'Moretti', 'Marco', '080901234', 'marco.moretti@example.com', 1),
(5, '37901234', 'Barbieri', 'Paolo', '070012345', 'paolo.barbieri@example.com', 1),
(6, '46012345', 'Fontana', 'Alberto', '032123456', 'alberto.fontana@example.com', 1),
(7, '27123456', 'Santoro', 'Nicola', '010234567', 'nicola.santoro@example.com', 1),
(8, '38234567', 'Mariani', 'Enrico', '051345678', 'enrico.mariani@example.com', 1),
(9, '47345678', 'Rinaldi', 'Federico', '081456789', 'federico.rinaldi@example.com', 1),
(10, '28456789', 'Caruso', 'Gabriele', '055567890', 'gabriele.caruso@example.com', 1),
(11, '39567890', 'Ferrara', 'Massimo', '091678901', 'massimo.ferrara@example.com', 1),
(12, '48678901', 'Martini', 'Diego', '011789012', 'diego.martini@example.com', 1),
(13, '29789012', 'Leone', 'Valerio', '040890123', 'valerio.leone@example.com', 1),
(14, '49890123', 'Longo', 'Samuele', '080901234', 'samuele.longo@example.com', 1),
(15, '30901234', 'Galli', 'Tommaso', '070012345', 'tommaso.galli@example.com', 1),
(16, '40012345', 'Benedetti', 'Filippo', '032123456', 'filippo.benedetti@example.com', 1),
(17, '31123456', 'Rossetti', 'Jacopo', '010234567', 'jacopo.rossetti@example.com', 1),
(18, '41234567', 'Marchesi', 'Edoardo', '051345678', 'edoardo.marchesi@example.com', 1),
(19, '32345678', 'Pellegrini', 'Leonardo', '081456789', 'leonardo.pellegrini@example.com', 1),
(20, '42456789', 'Palumbo', 'Cristiano', '055567890', 'cristiano.palumbo@example.com', 1),
(21, '33567890', 'Vitale', 'Daniele', '091678901', 'daniele.vitale@example.com', 1),
(22, '43678901', 'Silvestri', 'Luigi', '011789012', 'luigi.silvestri@example.com', 1),
(23, '34789012', 'Testa', 'Riccardo', '040890123', 'riccardo.testa@example.com', 1),
(26, '203456789', 'Rossi', 'Eduardo', '2657334756', 'rossi_2aa2@gmail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `IdPago` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Detalle` varchar(255) DEFAULT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `Estado` varchar(50) DEFAULT NULL,
  `IdUsuarioCrea` int(11) DEFAULT NULL,
  `IdUsuarioAnula` int(11) DEFAULT NULL,
  `FechaAnulacion` datetime DEFAULT NULL,
  `IdContrato` int(11) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`IdPago`, `NumeroPago`, `FechaPago`, `Detalle`, `Importe`, `Estado`, `IdUsuarioCrea`, `IdUsuarioAnula`, `FechaAnulacion`, `IdContrato`, `Activo`) VALUES
(1, 19, '2025-03-01', 'Mes 19', 30000.00, 'Activo', 1, NULL, NULL, 40, 1),
(2, 18, '2025-03-01', 'Mes 18', 28000.00, 'Activo', 2, NULL, NULL, 41, 1),
(3, 17, '2025-03-01', 'Mes 17', 38000.00, 'Activo', 1, NULL, NULL, 42, 1),
(4, 16, '2025-03-01', 'Mes 16', 33000.00, 'Activo', 2, NULL, NULL, 43, 1),
(5, 15, '2025-03-01', 'Mes 15', 36000.00, 'Activo', 1, NULL, NULL, 44, 1),
(8, 1, '2020-01-01', 'Mes 1', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(9, 1, '2021-03-01', 'Mes 1', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(10, 1, '2019-06-01', 'Mes 1', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(11, 1, '2022-07-01', 'Mes 1', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(12, 1, '2018-09-01', 'Mes 1', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(13, 2, '2020-02-01', 'Mes 2', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(14, 2, '2021-04-01', 'Mes 2', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(15, 2, '2019-07-01', 'Mes 2', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(16, 2, '2022-08-01', 'Mes 2', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(17, 2, '2018-10-01', 'Mes 2', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(18, 3, '2020-03-01', 'Mes 3', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(19, 3, '2021-05-01', 'Mes 3', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(20, 3, '2019-08-01', 'Mes 3', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(21, 3, '2022-09-01', 'Mes 3', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(22, 3, '2018-11-01', 'Mes 3', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(23, 4, '2020-04-01', 'Mes 4', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(24, 4, '2021-06-01', 'Mes 4', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(25, 4, '2019-09-01', 'Mes 4', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(26, 4, '2022-10-01', 'Mes 4', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(27, 4, '2018-12-01', 'Mes 4', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(28, 5, '2020-05-01', 'Mes 5', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(29, 5, '2021-07-01', 'Mes 5', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(30, 5, '2019-10-01', 'Mes 5', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(31, 5, '2022-11-01', 'Mes 5', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(32, 5, '2019-01-01', 'Mes 5', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(33, 6, '2020-06-01', 'Mes 6', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(34, 6, '2021-08-01', 'Mes 6', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(35, 6, '2019-11-01', 'Mes 6', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(36, 6, '2022-12-01', 'Mes 6', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(37, 6, '2019-02-01', 'Mes 6', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(38, 7, '2020-07-01', 'Mes 7', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(39, 7, '2021-09-01', 'Mes 7', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(40, 7, '2019-12-01', 'Mes 7', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(41, 7, '2023-01-01', 'Mes 7', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(42, 7, '2019-03-01', 'Mes 7', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(43, 8, '2020-08-01', 'Mes 8', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(44, 8, '2021-10-01', 'Mes 8', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(45, 8, '2020-01-01', 'Mes 8', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(46, 8, '2023-02-01', 'Mes 8', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(47, 8, '2019-04-01', 'Mes 8', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(48, 9, '2020-09-01', 'Mes 9', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(49, 9, '2021-11-01', 'Mes 9', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(50, 9, '2020-02-01', 'Mes 9', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(51, 9, '2023-03-01', 'Mes 9', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(52, 9, '2019-05-01', 'Mes 9', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(53, 10, '2020-10-01', 'Mes 10', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(54, 10, '2021-12-01', 'Mes 10', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(55, 10, '2020-03-01', 'Mes 10', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(56, 10, '2023-04-01', 'Mes 10', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(57, 10, '2019-06-01', 'Mes 10', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(58, 11, '2020-11-01', 'Mes 11', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(59, 11, '2022-01-01', 'Mes 11', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(60, 11, '2020-04-01', 'Mes 11', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(61, 11, '2023-05-01', 'Mes 11', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(62, 11, '2019-07-01', 'Mes 11', 36000.00, 'Activo', 1, NULL, NULL, 49, 1),
(63, 12, '2020-12-01', 'Mes 12', 40000.00, 'Activo', 1, NULL, NULL, 45, 1),
(64, 12, '2022-02-01', 'Mes 12', 50000.00, 'Activo', 2, NULL, NULL, 46, 1),
(65, 12, '2020-05-01', 'Mes 12', 32000.00, 'Activo', 1, NULL, NULL, 47, 1),
(66, 12, '2023-06-01', 'Mes 12', 42000.00, 'Activo', 2, NULL, NULL, 48, 1),
(67, 12, '2019-08-01', 'Mes 12', 36000.00, 'Activo', 1, NULL, NULL, 49, 1);

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
  `Email` varchar(100) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`IdPropietario`, `DNI`, `Apellido`, `Nombre`, `Telefono`, `Email`, `Activo`) VALUES
(1, '20345678', 'Rossi', 'Maria', '021234567', 'mario.rossi@example.com', 1),
(2, '31234567', 'Russo', 'Luca', '062345678', 'luca.russo@example.com', 1),
(3, '40987654', 'Ferrari', 'Giovanni', '041345678', 'giovanni.ferrari@example.com', 1),
(4, '21345678', 'Esposito', 'Antonio', '081456789', 'antonio.esposito@example.com', 1),
(5, '32456789', 'Bianchi', 'Francesco', '055567890', 'francesco.bianchi@example.com', 1),
(6, '41567890', 'Romano', 'Giuseppe', '091678901', 'giuseppe.romano@example.com', 1),
(7, '22678901', 'Colombo', 'Andrea', '011789012', 'andrea.colombo@example.com', 1),
(8, '33789012', 'Ricci', 'Matteo', '040890123', 'matteo.ricci@example.com', 1),
(9, '42890123', 'Marino', 'Lorenzo', '080901234', 'lorenzo.marino@example.com', 1),
(10, '23901234', 'Greco', 'Vincenzo', '070012345', 'vincenzo.greco@example.com', 1),
(11, '34012345', 'Bruno', 'Stefano', '032123456', 'stefano.bruno@example.com', 1),
(12, '43123456', 'Gallo', 'Alessandro', '010234567', 'alessandro.gallo@example.com', 1),
(13, '24234567', 'Conti', 'Simone', '051345678', 'simone.conti@example.com', 1),
(14, '35345678', 'Mancini', 'Davide', '081456789', 'davide.mancini@example.com', 1),
(15, '44456789', 'Costa', 'Fabio', '055567890', 'fabio.costa@example.com', 1),
(19, '25486955', 'Perez', 'Juan', '2657489877', 'perezj_22@gmail.com', 1),
(22, '254856664', 'Messi', 'Lionel', '312302131', 'lioMessi_10@gmail.com', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `IdTipoInmueble` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`IdTipoInmueble`, `Nombre`, `Activo`) VALUES
(1, 'Casa', 1),
(2, 'Departamento', 1),
(3, 'Local', 1),
(4, 'Depósito', 1),
(5, 'Local', 1),
(6, 'Depósito', 1),
(7, 'Casa', 1),
(8, 'Departamento', 1),
(9, 'Oficina', 1),
(10, 'Galpón', 1),
(11, 'Coworking', 1);

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
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`IdUsuario`, `Email`, `Contrasena`, `Rol`, `Avatar`, `FechaCreacion`, `Activo`) VALUES
(1, 'admin1@example.com', 'admin123', 'Administrador', 'https://example.com/avatars/admin1.jpg', '2025-03-25 20:01:08', 1),
(2, 'empleado1@example.com', 'empleado123', 'Empleado', 'https://example.com/avatars/empleado1.jpg', '2025-03-25 20:01:08', 1),
(3, 'empleado2@example.com', 'empleado456', 'Empleado', 'https://example.com/avatars/empleado2.jpg', '2025-03-25 20:01:08', 1),
(4, 'admin2@example.com', 'admin456', 'Administrador', 'https://example.com/avatars/admin2.jpg', '2025-03-25 20:01:08', 1),
(5, 'empleado3@example.com', 'empleado789', 'Empleado', NULL, '2025-03-25 20:01:08', 1);

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
  ADD KEY `fk_contrato_idusuariocrea` (`IdUsuarioCrea`),
  ADD KEY `fk_contrato_idusuarioanula` (`IdUsuarioAnula`);

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
  ADD KEY `pago_ibfk_1` (`IdContrato`),
  ADD KEY `fk_pago_idusuariocrea` (`IdUsuarioCrea`),
  ADD KEY `fk_pago_idusuarioanula` (`IdUsuarioAnula`);

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
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=64;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `IdTipoInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

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
  ADD CONSTRAINT `fk_contrato_idusuarioanula` FOREIGN KEY (`IdUsuarioAnula`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `fk_contrato_idusuariocrea` FOREIGN KEY (`IdUsuarioCrea`) REFERENCES `usuario` (`IdUsuario`);

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
  ADD CONSTRAINT `fk_pago_idusuarioanula` FOREIGN KEY (`IdUsuarioAnula`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `fk_pago_idusuariocrea` FOREIGN KEY (`IdUsuarioCrea`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`IdContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
