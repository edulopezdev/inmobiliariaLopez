-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 23-04-2025 a las 16:32:33
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
  `Activo` tinyint(1) NOT NULL DEFAULT 1,
  `EstadoContrato` enum('Vigente','Finalizado','Anulado','PendienteAnulacion') DEFAULT 'Vigente',
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`IdContrato`, `IdUsuarioCrea`, `FechaCreacion`, `IdUsuarioAnula`, `FechaRescision`, `FechaUsuarioAnula`, `IdInmueble`, `IdInquilino`, `FechaInicio`, `FechaFin`, `MontoMensual`, `Activo`, `EstadoContrato`, `Observaciones`) VALUES
(40, 1, '0001-01-01 00:00:00', 2, '2025-05-02', '2025-04-15 15:25:54', 1, 1, '2023-09-01', '2026-11-27', 1235555.00, 1, 'PendienteAnulacion', ''),
(41, 2, '2023-09-30 00:00:00', 2, '2025-05-30', '2025-04-15 12:34:15', 5, 2, '2023-10-16', '2025-09-30', 25000.00, 1, 'PendienteAnulacion', NULL),
(42, 1, '2023-10-29 00:00:00', 5, '2025-06-20', '2025-04-15 13:33:36', 8, 1, '2023-11-01', '2027-10-31', 38000.00, 1, 'Anulado', NULL),
(43, 2, '2023-11-28 00:00:00', 2, '2025-04-16', '2025-04-23 09:53:28', 11, 2, '2023-12-01', '2027-11-30', 33000.00, 1, 'Anulado', NULL),
(44, 1, '2023-12-27 00:00:00', 5, '2025-06-21', '2025-04-15 14:23:10', 14, 1, '2024-01-01', '2027-12-31', 36000.00, 1, 'PendienteAnulacion', 'probando de nuevo'),
(45, 1, '2019-12-29 00:00:00', 2, '2025-06-23', '2025-04-15 17:25:21', 2, 2, '2020-01-01', '2020-12-31', 40000.00, 1, 'PendienteAnulacion', 'El inquilino necesita dejar la casa porque tiene problemas económicos '),
(46, 2, '2021-02-28 00:00:00', 2, '2022-01-31', '2025-04-23 10:45:00', 3, 1, '2021-03-01', '2022-02-28', 50000.00, 1, 'Anulado', 'Roberto solicita irse un mes antes'),
(47, 1, '2019-05-26 00:00:00', 3, '2025-07-30', '2025-04-16 15:13:36', 7, 2, '2019-06-01', '2020-05-31', 32000.00, 1, 'PendienteAnulacion', 'Se va a partir de Julio'),
(48, 2, '2022-06-24 00:00:00', NULL, NULL, NULL, 10, 1, '2022-07-01', '2023-06-30', 42000.00, 1, 'Vigente', NULL),
(49, 1, '2018-08-25 00:00:00', NULL, NULL, NULL, 14, 2, '2018-09-01', '2019-08-31', 36000.00, 1, 'Vigente', NULL),
(52, 1, '2024-12-31 00:00:00', NULL, NULL, NULL, 2, 1, '2025-01-01', '2025-04-15', 40000.00, 1, 'Finalizado', NULL),
(53, 1, '2025-01-27 00:00:00', NULL, NULL, NULL, 3, 2, '2025-02-01', '2025-04-30', 50000.00, 1, 'Vigente', NULL),
(60, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 17, 2, '2025-04-12', '2026-04-12', 155000.00, 1, 'Vigente', NULL),
(61, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 9, 18, '2025-04-12', '2026-04-12', 152400.00, 1, 'Vigente', NULL),
(62, 1, '0001-01-01 00:00:00', NULL, NULL, NULL, 9, 23, '2026-04-13', '2027-04-11', 444000.00, 1, 'Vigente', NULL),
(63, 1, '0001-01-01 00:00:00', 2, '2025-03-29', '2025-02-16 13:13:36', 17, 10, '2025-04-14', '2026-04-14', 555555.00, 1, 'Anulado', 'El inquilino pidio irse antes'),
(64, 2, '0001-01-01 00:00:00', 5, '2025-04-19', '2025-04-19 16:18:41', 16, 22, '2025-04-19', '2026-04-19', 865340.00, 1, 'PendienteAnulacion', 'Se va porque le pinto'),
(79, 1, '2025-04-23 09:41:47', NULL, NULL, NULL, 16, 13, '2028-04-19', '2030-05-30', 850000.00, 1, 'Vigente', NULL);

--
-- Disparadores `contrato`
--
DELIMITER $$
CREATE TRIGGER `trigger_calcular_multa` AFTER UPDATE ON `contrato` FOR EACH ROW BEGIN
    -- Declaración de las variables al principio
    DECLARE dias_cumplidos INT;
    DECLARE dias_totales INT;
    DECLARE multa DECIMAL(10,2);
    DECLARE multa_anticipada DECIMAL(10,2);
    DECLARE multa_tardia DECIMAL(10,2);

    -- Verifica si se ha actualizado la fecha de rescisión (FechaRescision) y si es antes de la FechaFin
    IF NEW.FechaRescision IS NOT NULL AND NEW.FechaRescision < OLD.FechaFin THEN

        -- Calcula los días cumplidos del contrato
        SET dias_cumplidos = DATEDIFF(NEW.FechaRescision, OLD.FechaInicio);
        SET dias_totales = DATEDIFF(OLD.FechaFin, OLD.FechaInicio);

        -- Se asume que hay reglas de multa configuradas en la tabla de configuracion_multa
        -- Si tienes una tabla de configuracion, este es el lugar donde las buscarías
        -- Por ahora lo dejo fijo para ilustrar el cálculo.

        -- Ejemplo: multa anticipada 2 meses de alquiler, multa tardía 1 mes de alquiler
        SET multa_anticipada = 2.00;  -- Ejemplo: 2 meses de alquiler
        SET multa_tardia = 1.00;  -- Ejemplo: 1 mes de alquiler

        -- Cálculo de la multa basado en la fecha de rescisión
        IF dias_cumplidos < dias_totales / 2 THEN
            -- Rescisión anticipada
            SET multa = multa_anticipada * OLD.MontoMensual;  -- 2 meses de alquiler
        ELSE
            -- Rescisión tardía
            SET multa = multa_tardia * OLD.MontoMensual;  -- 1 mes de alquiler
        END IF;

        -- Inserta la multa en la tabla 'multa'
        INSERT INTO multa (IdContrato, Monto, Motivo, Pagada)
        VALUES (NEW.IdContrato, multa, 'Multa por cancelación anticipada', 0);  -- 0 indica que la multa no está pagada

    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagen`
--

CREATE TABLE `imagen` (
  `IdImagen` int(11) NOT NULL,
  `Ruta` varchar(255) NOT NULL,
  `TipoImagen` enum('InmueblePortada','InmuebleInterior','AvatarUsuario','AdjuntoContrato','AdjuntoPago') NOT NULL,
  `IdRelacionado` int(11) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `Activo` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `imagen`
--

INSERT INTO `imagen` (`IdImagen`, `Ruta`, `TipoImagen`, `IdRelacionado`, `FechaCreacion`, `Activo`) VALUES
(71, '/img/inmuebles/galeria/inmuebleinterior_1_2207cf5a.png', 'InmuebleInterior', 1, '2025-04-18 22:20:00', 1),
(83, '/img/inmuebles/portada/inmuebleportada_17_923104e7.png', 'InmueblePortada', 17, '2025-04-22 15:29:26', 1),
(84, '/img/inmuebles/galeria/inmuebleinterior_17_7f5e5d01.png', 'InmuebleInterior', 17, '2025-04-22 15:29:37', 1),
(85, '/img/inmuebles/galeria/inmuebleinterior_17_b37da083.png', 'InmuebleInterior', 17, '2025-04-22 15:29:37', 1),
(86, '/img/inmuebles/galeria/inmuebleinterior_17_d14ed3b6.png', 'InmuebleInterior', 17, '2025-04-22 15:29:37', 1),
(88, '/img/inmuebles/portada/inmuebleportada_2_a88b7036.png', 'InmueblePortada', 2, '2025-04-22 15:30:14', 1),
(89, '/img/inmuebles/galeria/inmuebleinterior_2_7fffc56f.png', 'InmuebleInterior', 2, '2025-04-22 15:30:36', 1),
(90, '/img/inmuebles/galeria/inmuebleinterior_2_01f83941.png', 'InmuebleInterior', 2, '2025-04-22 15:30:36', 1),
(91, '/img/inmuebles/galeria/inmuebleinterior_2_99585c70.png', 'InmuebleInterior', 2, '2025-04-22 15:30:36', 1),
(92, '/img/inmuebles/portada/inmuebleportada_18_de5efc00.png', 'InmueblePortada', 18, '2025-04-22 15:31:29', 1),
(93, '/img/inmuebles/galeria/inmuebleinterior_18_87a4f2cc.png', 'InmuebleInterior', 18, '2025-04-22 15:31:38', 1),
(94, '/img/inmuebles/galeria/inmuebleinterior_18_00fa9bad.png', 'InmuebleInterior', 18, '2025-04-22 15:31:38', 1),
(96, '/img/inmuebles/portada/inmuebleportada_3_016a9d7e.png', 'InmueblePortada', 3, '2025-04-22 15:34:25', 1),
(97, '/img/inmuebles/galeria/inmuebleinterior_3_ddd3cb72.png', 'InmuebleInterior', 3, '2025-04-22 15:34:39', 1),
(98, '/img/inmuebles/galeria/inmuebleinterior_3_575bb567.png', 'InmuebleInterior', 3, '2025-04-22 15:34:39', 1),
(99, '/img/inmuebles/galeria/inmuebleinterior_3_8f6da364.png', 'InmuebleInterior', 3, '2025-04-22 15:34:39', 1),
(100, '/img/inmuebles/portada/inmuebleportada_19_78a127f7.png', 'InmueblePortada', 19, '2025-04-22 15:38:41', 1),
(101, '/img/inmuebles/galeria/inmuebleinterior_19_6f0543dd.png', 'InmuebleInterior', 19, '2025-04-22 15:38:55', 1),
(102, '/img/inmuebles/galeria/inmuebleinterior_19_ec21c5f2.png', 'InmuebleInterior', 19, '2025-04-22 15:38:55', 1),
(103, '/img/inmuebles/galeria/inmuebleinterior_19_0f669b66.png', 'InmuebleInterior', 19, '2025-04-22 15:38:55', 1),
(105, '/img/inmuebles/portada/inmuebleportada_4_2e1821f6.png', 'InmueblePortada', 4, '2025-04-22 15:45:07', 1),
(106, '/img/inmuebles/portada/inmuebleportada_20_ad6fe9da.png', 'InmueblePortada', 20, '2025-04-22 15:48:48', 1),
(107, '/img/inmuebles/galeria/inmuebleinterior_20_2d5e38a4.png', 'InmuebleInterior', 20, '2025-04-22 15:49:20', 1),
(108, '/img/inmuebles/galeria/inmuebleinterior_20_4f4402ad.png', 'InmuebleInterior', 20, '2025-04-22 15:49:20', 1),
(109, '/img/inmuebles/galeria/inmuebleinterior_20_5d445449.png', 'InmuebleInterior', 20, '2025-04-22 15:49:20', 1),
(110, '/img/inmuebles/galeria/inmuebleinterior_5_1dbf83f1.png', 'InmuebleInterior', 5, '2025-04-22 15:57:36', 1),
(111, '/img/inmuebles/galeria/inmuebleinterior_5_03fb6dd0.png', 'InmuebleInterior', 5, '2025-04-22 15:57:36', 1),
(112, '/img/inmuebles/galeria/inmuebleinterior_5_f26a6bbd.png', 'InmuebleInterior', 5, '2025-04-22 15:57:36', 1),
(113, '/img/inmuebles/portada/inmuebleportada_5_eddc6418.png', 'InmueblePortada', 5, '2025-04-22 15:57:41', 1),
(114, '/img/inmuebles/portada/inmuebleportada_21_5406ac57.png', 'InmueblePortada', 21, '2025-04-22 15:58:06', 1),
(115, '/img/inmuebles/galeria/inmuebleinterior_21_a66c4129.png', 'InmuebleInterior', 21, '2025-04-22 15:58:13', 1),
(116, '/img/inmuebles/galeria/inmuebleinterior_21_edac2ec5.png', 'InmuebleInterior', 21, '2025-04-22 15:58:13', 1),
(117, '/img/inmuebles/galeria/inmuebleinterior_21_927edf4d.png', 'InmuebleInterior', 21, '2025-04-22 15:58:13', 1),
(118, '/img/inmuebles/portada/inmuebleportada_6_e11e8d29.png', 'InmueblePortada', 6, '2025-04-22 16:00:02', 1),
(119, '/img/inmuebles/galeria/inmuebleinterior_6_63af647d.png', 'InmuebleInterior', 6, '2025-04-22 16:00:11', 1),
(120, '/img/inmuebles/galeria/inmuebleinterior_6_02d3e4b8.png', 'InmuebleInterior', 6, '2025-04-22 16:00:11', 1),
(121, '/img/inmuebles/galeria/inmuebleinterior_6_eb8fcfc9.png', 'InmuebleInterior', 6, '2025-04-22 16:00:11', 1),
(122, '/img/inmuebles/portada/inmuebleportada_22_05d367d9.png', 'InmueblePortada', 22, '2025-04-22 16:10:50', 1),
(123, '/img/inmuebles/galeria/inmuebleinterior_22_c5d99697.png', 'InmuebleInterior', 22, '2025-04-22 16:10:58', 1),
(124, '/img/inmuebles/galeria/inmuebleinterior_22_a546d402.png', 'InmuebleInterior', 22, '2025-04-22 16:10:58', 1),
(125, '/img/inmuebles/galeria/inmuebleinterior_22_93ba0e12.png', 'InmuebleInterior', 22, '2025-04-22 16:10:58', 1),
(126, '/img/inmuebles/portada/inmuebleportada_7_7c8436ac.png', 'InmueblePortada', 7, '2025-04-22 16:11:19', 1),
(127, '/img/inmuebles/galeria/inmuebleinterior_7_b4cc433a.png', 'InmuebleInterior', 7, '2025-04-22 16:11:29', 1),
(128, '/img/inmuebles/galeria/inmuebleinterior_7_74bc548c.png', 'InmuebleInterior', 7, '2025-04-22 16:11:29', 1),
(129, '/img/inmuebles/galeria/inmuebleinterior_7_cf0c30a8.png', 'InmuebleInterior', 7, '2025-04-22 16:11:29', 1),
(130, '/img/inmuebles/portada/inmuebleportada_23_de6cc9ba.png', 'InmueblePortada', 23, '2025-04-22 16:11:49', 1),
(131, '/img/inmuebles/galeria/inmuebleinterior_23_40bca5ef.png', 'InmuebleInterior', 23, '2025-04-22 16:12:00', 1),
(132, '/img/inmuebles/galeria/inmuebleinterior_23_1e2c3d01.png', 'InmuebleInterior', 23, '2025-04-22 16:12:00', 1),
(133, '/img/inmuebles/galeria/inmuebleinterior_23_c63212af.png', 'InmuebleInterior', 23, '2025-04-22 16:12:00', 1),
(134, '/img/inmuebles/portada/inmuebleportada_8_929be5e6.png', 'InmueblePortada', 8, '2025-04-22 16:12:28', 1),
(135, '/img/inmuebles/galeria/inmuebleinterior_8_887d2158.png', 'InmuebleInterior', 8, '2025-04-22 16:12:35', 1),
(136, '/img/inmuebles/galeria/inmuebleinterior_8_d91e2bae.png', 'InmuebleInterior', 8, '2025-04-22 16:12:35', 1),
(137, '/img/inmuebles/galeria/inmuebleinterior_8_67a66b9e.png', 'InmuebleInterior', 8, '2025-04-22 16:12:35', 1),
(138, '/img/inmuebles/portada/inmuebleportada_24_c427ed5d.png', 'InmueblePortada', 24, '2025-04-22 16:20:51', 1),
(139, '/img/inmuebles/galeria/inmuebleinterior_24_8457b33e.png', 'InmuebleInterior', 24, '2025-04-22 16:21:00', 1),
(140, '/img/inmuebles/galeria/inmuebleinterior_24_44052999.png', 'InmuebleInterior', 24, '2025-04-22 16:21:00', 1),
(141, '/img/inmuebles/galeria/inmuebleinterior_24_1f6a2acc.png', 'InmuebleInterior', 24, '2025-04-22 16:21:00', 1),
(142, '/img/inmuebles/portada/inmuebleportada_25_a6665fe6.png', 'InmueblePortada', 25, '2025-04-22 16:28:55', 1),
(143, '/img/inmuebles/galeria/inmuebleinterior_25_0e58c117.png', 'InmuebleInterior', 25, '2025-04-22 16:29:02', 1),
(144, '/img/inmuebles/galeria/inmuebleinterior_25_12cc393a.png', 'InmuebleInterior', 25, '2025-04-22 16:29:02', 1),
(145, '/img/inmuebles/galeria/inmuebleinterior_25_0ce88765.png', 'InmuebleInterior', 25, '2025-04-22 16:29:02', 1),
(146, '/img/inmuebles/portada/inmuebleportada_10_f6e35b56.png', 'InmueblePortada', 10, '2025-04-22 16:29:26', 1),
(147, '/img/inmuebles/galeria/inmuebleinterior_10_3d6ae982.png', 'InmuebleInterior', 10, '2025-04-22 16:29:33', 1),
(148, '/img/inmuebles/galeria/inmuebleinterior_10_d0020422.png', 'InmuebleInterior', 10, '2025-04-22 16:29:33', 1),
(149, '/img/inmuebles/galeria/inmuebleinterior_10_4bc9167b.png', 'InmuebleInterior', 10, '2025-04-22 16:29:33', 1),
(150, '/img/inmuebles/portada/inmuebleportada_26_149720e9.png', 'InmueblePortada', 26, '2025-04-22 16:29:55', 1),
(151, '/img/inmuebles/galeria/inmuebleinterior_26_8157a417.png', 'InmuebleInterior', 26, '2025-04-22 16:30:02', 1),
(152, '/img/inmuebles/galeria/inmuebleinterior_26_e6ba670f.png', 'InmuebleInterior', 26, '2025-04-22 16:30:02', 1),
(153, '/img/inmuebles/galeria/inmuebleinterior_26_772610d1.png', 'InmuebleInterior', 26, '2025-04-22 16:30:02', 1),
(154, '/img/inmuebles/portada/inmuebleportada_11_759ecafd.png', 'InmueblePortada', 11, '2025-04-22 16:30:45', 1),
(155, '/img/inmuebles/galeria/inmuebleinterior_11_b11f1bc3.png', 'InmuebleInterior', 11, '2025-04-22 16:30:52', 1),
(156, '/img/inmuebles/galeria/inmuebleinterior_11_cdd43f2d.png', 'InmuebleInterior', 11, '2025-04-22 16:30:52', 1),
(157, '/img/inmuebles/galeria/inmuebleinterior_11_d289866d.png', 'InmuebleInterior', 11, '2025-04-22 16:30:52', 1),
(158, '/img/inmuebles/portada/inmuebleportada_27_fb34672e.png', 'InmueblePortada', 27, '2025-04-22 16:31:29', 1),
(159, '/img/inmuebles/galeria/inmuebleinterior_27_5433e376.png', 'InmuebleInterior', 27, '2025-04-22 16:31:35', 1),
(160, '/img/inmuebles/galeria/inmuebleinterior_27_068598e1.png', 'InmuebleInterior', 27, '2025-04-22 16:31:35', 1),
(161, '/img/inmuebles/galeria/inmuebleinterior_27_22a356b5.png', 'InmuebleInterior', 27, '2025-04-22 16:31:35', 1),
(162, '/img/inmuebles/portada/inmuebleportada_12_f6454ae1.png', 'InmueblePortada', 12, '2025-04-22 16:31:50', 1),
(163, '/img/inmuebles/galeria/inmuebleinterior_12_9a826031.png', 'InmuebleInterior', 12, '2025-04-22 16:31:57', 1),
(164, '/img/inmuebles/galeria/inmuebleinterior_12_3d09231e.png', 'InmuebleInterior', 12, '2025-04-22 16:31:57', 1),
(165, '/img/inmuebles/galeria/inmuebleinterior_12_b7eb144e.png', 'InmuebleInterior', 12, '2025-04-22 16:31:57', 1),
(166, '/img/inmuebles/portada/inmuebleportada_13_083a80dd.png', 'InmueblePortada', 13, '2025-04-22 16:32:17', 1),
(167, '/img/inmuebles/galeria/inmuebleinterior_13_a277b6b8.png', 'InmuebleInterior', 13, '2025-04-22 16:32:27', 1),
(168, '/img/inmuebles/galeria/inmuebleinterior_13_1eba6aa4.png', 'InmuebleInterior', 13, '2025-04-22 16:32:27', 1),
(169, '/img/inmuebles/galeria/inmuebleinterior_13_c9b592fc.png', 'InmuebleInterior', 13, '2025-04-22 16:32:27', 1),
(170, '/img/inmuebles/portada/inmuebleportada_14_4b5ccacf.png', 'InmueblePortada', 14, '2025-04-22 16:32:41', 1),
(171, '/img/inmuebles/galeria/inmuebleinterior_14_4d170acf.png', 'InmuebleInterior', 14, '2025-04-22 16:32:48', 1),
(172, '/img/inmuebles/galeria/inmuebleinterior_14_0a6f17bc.png', 'InmuebleInterior', 14, '2025-04-22 16:32:48', 1),
(173, '/img/inmuebles/galeria/inmuebleinterior_14_2e8895a6.png', 'InmuebleInterior', 14, '2025-04-22 16:32:48', 1),
(174, '/img/inmuebles/portada/inmuebleportada_15_6f4634d5.png', 'InmueblePortada', 15, '2025-04-22 16:33:06', 1),
(175, '/img/inmuebles/galeria/inmuebleinterior_15_9301432f.png', 'InmuebleInterior', 15, '2025-04-22 16:33:13', 1),
(176, '/img/inmuebles/galeria/inmuebleinterior_15_f1e6f903.png', 'InmuebleInterior', 15, '2025-04-22 16:33:13', 1),
(177, '/img/inmuebles/galeria/inmuebleinterior_15_04af2c47.png', 'InmuebleInterior', 15, '2025-04-22 16:33:13', 1),
(178, '/img/inmuebles/portada/inmuebleportada_16_e4f1eaf4.png', 'InmueblePortada', 16, '2025-04-22 16:33:29', 1),
(179, '/img/inmuebles/galeria/inmuebleinterior_16_2270174c.png', 'InmuebleInterior', 16, '2025-04-22 16:33:42', 1),
(180, '/img/inmuebles/galeria/inmuebleinterior_16_5e8103e2.png', 'InmuebleInterior', 16, '2025-04-22 16:33:42', 1),
(181, '/img/inmuebles/galeria/inmuebleinterior_16_a4568d38.png', 'InmuebleInterior', 16, '2025-04-22 16:33:42', 1),
(182, '/img/inmuebles/portada/inmuebleportada_9_4478f78d.png', 'InmueblePortada', 9, '2025-04-22 16:41:12', 1),
(183, '/img/inmuebles/galeria/inmuebleinterior_9_a9cb0069.jpeg', 'InmuebleInterior', 9, '2025-04-22 16:41:16', 1),
(184, '/img/inmuebles/galeria/inmuebleinterior_9_cb85b619.jpg', 'InmuebleInterior', 9, '2025-04-22 16:41:16', 1),
(185, '/img/inmuebles/galeria/inmuebleinterior_9_90b3ea90.jpeg', 'InmuebleInterior', 9, '2025-04-22 16:41:16', 1),
(210, '/img/inmuebles/portada/inmuebleportada_34_d5354624.png', 'InmueblePortada', 34, '2025-04-22 19:32:53', 1),
(211, '/img/inmuebles/galeria/inmuebleinterior_34_5a34b984.png', 'InmuebleInterior', 34, '2025-04-22 19:32:59', 1),
(212, '/img/inmuebles/galeria/inmuebleinterior_34_20f0cdd2.png', 'InmuebleInterior', 34, '2025-04-22 19:32:59', 1),
(213, '/img/inmuebles/portada/inmuebleportada_0_74a0099f.jpg', 'InmueblePortada', 0, '2025-04-22 19:39:16', 1),
(214, '/img/inmuebles/portada/inmuebleportada_36_b491afe9.png', 'InmueblePortada', 36, '2025-04-22 19:49:16', 1),
(215, '/img/inmuebles/galeria/inmuebleinterior_36_c8bc2a8c.png', 'InmuebleInterior', 36, '2025-04-22 19:57:24', 1),
(216, '/img/inmuebles/galeria/inmuebleinterior_36_7581622e.png', 'InmuebleInterior', 36, '2025-04-22 19:57:24', 1),
(221, '/img/inmuebles/galeria/inmuebleinterior_1_906f0382.png', 'InmuebleInterior', 1, '2025-04-22 22:33:40', 1),
(243, '/img/inmuebles/portada/inmuebleportada_37_7ddadb02.png', 'InmueblePortada', 37, '2025-04-23 00:14:03', 1),
(244, '/img/inmuebles/galeria/inmuebleinterior_37_9359d21f.png', 'InmuebleInterior', 37, '2025-04-23 00:14:10', 1),
(245, '/img/inmuebles/galeria/inmuebleinterior_37_5e42ecfb.png', 'InmuebleInterior', 37, '2025-04-23 00:14:10', 1),
(247, '/img/inmuebles/galeria/inmuebleinterior_18_3317133e.png', 'InmuebleInterior', 18, '2025-04-23 00:33:52', 1),
(248, '/img/inmuebles/galeria/inmuebleinterior_1_6e0973ce.png', 'InmuebleInterior', 1, '2025-04-23 00:34:17', 1),
(250, '/img/inmuebles/portada/inmuebleportada_1_67984f39.png', 'InmueblePortada', 1, '2025-04-23 00:34:40', 1);

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
(1, 'Via Montenapoleone 11, Milano', 'Residencial', 3, 4, 180, 12.000000, 12.000000, 545000.00, 'Disponible', 22, 1),
(2, 'Piazza del Duomo 5, Milano', 'Comercial', 1, 1, 40, 45.460000, 9.190000, 40000.00, 'Disponible', 13, 1),
(3, 'Corso Buenos Aires 45, Milano', 'Residencial', 3, 5, 250, 45.473600, 9.218100, 50000.00, 'Disponible', 3, 1),
(4, 'Via del Corso 23, Roma', 'Comercial', 1, 1, 40, 41.898700, 12.482800, 35000.00, 'Disponible', 4, 1),
(5, 'Piazza Navona 8, Roma', 'Residencial', 3, 4, 120, 165.000000, 1525.000000, 1555.00, 'Disponible', 12, 1),
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
(17, 'Piazza Castello 3, Torino', 'Comercial', 2, 2, 120, 152.000000, 1551.000000, 155000.00, 'Disponible', 19, 1),
(18, 'Corso Vittorio Emanuele II 25, Torino', 'Residencial', 3, 4, 200, 45.070000, 7.680000, 6658.00, 'Disponible', 2, 1),
(19, 'Piazza Maggiore 5, Bologna', 'Comercial', 1, 1, 40, 44.494900, 11.342600, 34000.00, 'Disponible', 3, 1),
(20, 'Via Rizzoli 12, Bologna', 'Residencial', 4, 3, 180, 44.493800, 11.342600, 37000.00, 'Disponible', 4, 1),
(21, 'Via dell\'Indipendenza 18, Bologna', 'Residencial', 3, 5, 250, 44.494900, 11.342600, 52000.00, 'Disponible', 5, 1),
(22, 'Via Garibaldi 8, Genova', 'Comercial', 1, 1, 40, 44.408200, 8.933800, 33000.00, 'Disponible', 6, 1),
(23, 'Piazza De Ferrari 4, Genova', 'Residencial', 4, 2, 120, 44.407300, 8.933800, 30000.00, 'Disponible', 7, 1),
(24, 'Via XX Settembre 10, Genova', 'Residencial', 3, 4, 200, 44.408200, 8.933800, 49000.00, 'Disponible', 8, 1),
(25, 'Piazza delle Erbe 3, Verona', 'Comercial', 1, 1, 40, 45.438400, 10.991900, 32000.00, 'Disponible', 9, 1),
(26, 'Via Mazzini 7, Verona', 'Residencial', 4, 3, 180, 45.439000, 10.991900, 35000.00, 'Disponible', 10, 1),
(27, 'Corso Porta Nuova 12, Verona', 'Residencial', 3, 5, 250, 45.438400, 10.991900, 50000.00, 'Disponible', 11, 1),
(36, 'Pruebitaaa 10', 'Residencial', 3, 4, 0, 40.712776, -74.005974, 250000.00, 'Disponible', 1, 0),
(37, '9 de Julio 814, Villa Mercedes', 'Comercial', 6, 4, 0, -155.000000, 15.000000, 650200.00, 'Disponible', 9, 0);

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
-- Estructura de tabla para la tabla `multa`
--

CREATE TABLE `multa` (
  `IdMulta` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `FechaCalculo` datetime NOT NULL DEFAULT current_timestamp(),
  `Motivo` varchar(500) DEFAULT NULL,
  `Pagada` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `multa`
--

INSERT INTO `multa` (`IdMulta`, `IdContrato`, `Monto`, `FechaCalculo`, `Motivo`, `Pagada`) VALUES
(1, 43, 66000.00, '2025-04-23 11:29:02', 'Multa por cancelación anticipada', 0),
(2, 42, 76000.00, '2025-04-23 11:29:02', 'Multa por cancelación anticipada', 0),
(3, 40, 1235555.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0),
(4, 41, 25000.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0),
(5, 44, 72000.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0),
(6, 46, 50000.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0),
(7, 63, 1111110.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0),
(8, 64, 1730680.00, '2025-04-23 11:31:04', 'Multa por cancelación anticipada', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `IdPago` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `TipoPago` enum('Alquiler','Multa','Otro') NOT NULL DEFAULT 'Alquiler',
  `MesesAdeudados` int(11) DEFAULT NULL,
  `Estado` enum('Pendiente','Pagado','Anulado') NOT NULL DEFAULT 'Pendiente',
  `Detalle` varchar(255) DEFAULT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `IdUsuarioCrea` int(11) DEFAULT NULL,
  `IdUsuarioAnula` int(11) DEFAULT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `FechaAnulacion` datetime DEFAULT NULL,
  `IdMulta` int(11) DEFAULT NULL,
  `IdContrato` int(11) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1,
  `MotivoAnulacion` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`IdPago`, `NumeroPago`, `FechaPago`, `TipoPago`, `MesesAdeudados`, `Estado`, `Detalle`, `Importe`, `IdUsuarioCrea`, `IdUsuarioAnula`, `FechaCreacion`, `FechaAnulacion`, `IdMulta`, `IdContrato`, `Activo`, `MotivoAnulacion`) VALUES
(1, 0, '2025-04-23', 'Alquiler', 1, 'Pagado', 'Primer mes de alquiler', 40500.00, 3, NULL, '2025-04-23 01:28:28', NULL, NULL, 40, 1, NULL),
(2, 18, '2025-03-01', 'Alquiler', NULL, 'Pagado', 'Mes 18', 28000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 41, 1, NULL),
(3, 17, '2025-03-01', 'Alquiler', NULL, 'Pagado', 'Mes 17', 38000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 42, 1, NULL),
(4, 16, '2025-03-01', 'Alquiler', NULL, 'Pagado', 'Mes 16', 33000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 43, 1, NULL),
(5, 15, '2025-03-01', 'Alquiler', NULL, 'Pagado', 'Mes 15', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 44, 1, NULL),
(8, 1, '2020-01-01', 'Alquiler', NULL, 'Pagado', 'Mes 1', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(9, 1, '2021-03-01', 'Alquiler', NULL, 'Pagado', 'Mes 1', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(10, 1, '2019-06-01', 'Alquiler', NULL, 'Pagado', 'Mes 1', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(11, 1, '2022-07-01', 'Alquiler', NULL, 'Pagado', 'Mes 1', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(12, 1, '2018-09-01', 'Alquiler', NULL, 'Pagado', 'Mes 1', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(13, 2, '2020-02-01', 'Alquiler', NULL, 'Pagado', 'Mes 2', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(14, 2, '2021-04-01', 'Alquiler', NULL, 'Pagado', 'Mes 2', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(15, 2, '2019-07-01', 'Alquiler', NULL, 'Pagado', 'Mes 2', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(16, 2, '2022-08-01', 'Alquiler', NULL, 'Pendiente', 'Mes 2', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(17, 2, '2018-10-01', 'Alquiler', NULL, 'Pendiente', 'Mes 2', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(18, 3, '2020-03-01', 'Alquiler', NULL, 'Pendiente', 'Mes 3', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(19, 3, '2021-05-01', 'Alquiler', NULL, 'Pendiente', 'Mes 3', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(20, 3, '2019-08-01', 'Alquiler', NULL, 'Pendiente', 'Mes 3', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(21, 3, '2022-09-01', 'Alquiler', NULL, 'Pendiente', 'Mes 3', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(22, 3, '2018-11-01', 'Alquiler', NULL, 'Pendiente', 'Mes 3', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(23, 4, '2020-04-01', 'Alquiler', NULL, 'Pendiente', 'Mes 4', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(24, 4, '2021-06-01', 'Alquiler', NULL, 'Pendiente', 'Mes 4', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(25, 4, '2019-09-01', 'Alquiler', NULL, 'Pendiente', 'Mes 4', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(26, 4, '2022-10-01', 'Alquiler', NULL, 'Pendiente', 'Mes 4', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(27, 4, '2018-12-01', 'Alquiler', NULL, 'Pendiente', 'Mes 4', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(28, 5, '2020-05-01', 'Alquiler', NULL, 'Pendiente', 'Mes 5', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(29, 5, '2021-07-01', 'Alquiler', NULL, 'Pendiente', 'Mes 5', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(30, 5, '2019-10-01', 'Alquiler', NULL, 'Pendiente', 'Mes 5', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(31, 5, '2022-11-01', 'Alquiler', NULL, 'Pendiente', 'Mes 5', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(32, 5, '2019-01-01', 'Alquiler', NULL, 'Pendiente', 'Mes 5', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(33, 6, '2020-06-01', 'Alquiler', NULL, 'Pendiente', 'Mes 6', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(34, 6, '2021-08-01', 'Alquiler', NULL, 'Pendiente', 'Mes 6', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(35, 6, '2019-11-01', 'Alquiler', NULL, 'Pendiente', 'Mes 6', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(36, 6, '2022-12-01', 'Alquiler', NULL, 'Pendiente', 'Mes 6', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(37, 6, '2019-02-01', 'Alquiler', NULL, 'Pendiente', 'Mes 6', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(38, 7, '2020-07-01', 'Alquiler', NULL, 'Pendiente', 'Mes 7', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(39, 7, '2021-09-01', 'Alquiler', NULL, 'Pendiente', 'Mes 7', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(40, 7, '2019-12-01', 'Alquiler', NULL, 'Pendiente', 'Mes 7', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(41, 7, '2023-01-01', 'Alquiler', NULL, 'Pendiente', 'Mes 7', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(42, 7, '2019-03-01', 'Alquiler', NULL, 'Pendiente', 'Mes 7', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(43, 8, '2020-08-01', 'Alquiler', NULL, 'Pendiente', 'Mes 8', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(44, 8, '2021-10-01', 'Alquiler', NULL, 'Pendiente', 'Mes 8', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(45, 8, '2020-01-01', 'Alquiler', NULL, 'Pendiente', 'Mes 8', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(46, 8, '2023-02-01', 'Alquiler', NULL, 'Pendiente', 'Mes 8', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(47, 8, '2019-04-01', 'Alquiler', NULL, 'Pendiente', 'Mes 8', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(48, 9, '2020-09-01', 'Alquiler', NULL, 'Pendiente', 'Mes 9', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(49, 9, '2021-11-01', 'Alquiler', NULL, 'Pendiente', 'Mes 9', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(50, 9, '2020-02-01', 'Alquiler', NULL, 'Pendiente', 'Mes 9', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(51, 9, '2023-03-01', 'Alquiler', NULL, 'Pendiente', 'Mes 9', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(52, 9, '2019-05-01', 'Alquiler', NULL, 'Pendiente', 'Mes 9', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(53, 10, '2020-10-01', 'Alquiler', NULL, 'Pendiente', 'Mes 10', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(54, 10, '2021-12-01', 'Alquiler', NULL, 'Pendiente', 'Mes 10', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(55, 10, '2020-03-01', 'Alquiler', NULL, 'Pendiente', 'Mes 10', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(56, 10, '2023-04-01', 'Alquiler', NULL, 'Pendiente', 'Mes 10', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(57, 10, '2019-06-01', 'Alquiler', NULL, 'Pendiente', 'Mes 10', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(58, 11, '2020-11-01', 'Alquiler', NULL, 'Pendiente', 'Mes 11', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(59, 11, '2022-01-01', 'Alquiler', NULL, 'Pendiente', 'Mes 11', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(60, 11, '2020-04-01', 'Alquiler', NULL, 'Pendiente', 'Mes 11', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(61, 11, '2023-05-01', 'Alquiler', NULL, 'Pendiente', 'Mes 11', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(62, 11, '2019-07-01', 'Alquiler', NULL, 'Pendiente', 'Mes 11', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(63, 12, '2020-12-01', 'Alquiler', NULL, 'Pendiente', 'Mes 12', 40000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 45, 1, NULL),
(64, 12, '2022-02-01', 'Alquiler', NULL, 'Pendiente', 'Mes 12', 50000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 46, 1, NULL),
(65, 12, '2020-05-01', 'Alquiler', NULL, 'Pendiente', 'Mes 12', 32000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 47, 1, NULL),
(66, 12, '2023-06-01', 'Alquiler', NULL, 'Pendiente', 'Mes 12', 42000.00, 2, NULL, '2025-04-19 13:22:42', NULL, NULL, 48, 1, NULL),
(67, 12, '2019-08-01', 'Alquiler', NULL, 'Pendiente', 'Mes 12', 36000.00, 1, NULL, '2025-04-19 13:22:42', NULL, NULL, 49, 1, NULL),
(68, 0, '2025-04-22', 'Alquiler', 2, 'Pendiente', 'Primer mes de alquiler', 65000.00, 4, NULL, '2025-04-23 01:38:30', NULL, NULL, 41, 1, NULL);

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
(7, 'Departamento', 1),
(8, 'Oficina', 1),
(9, 'Galpón', 1),
(10, 'Coworking', 1);

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
-- Indices de la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD PRIMARY KEY (`IdImagen`);

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
-- Indices de la tabla `multa`
--
ALTER TABLE `multa`
  ADD PRIMARY KEY (`IdMulta`),
  ADD KEY `IdContrato` (`IdContrato`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `IdContrato` (`IdContrato`),
  ADD KEY `IdUsuarioCrea` (`IdUsuarioCrea`),
  ADD KEY `IdUsuarioAnula` (`IdUsuarioAnula`),
  ADD KEY `IdMulta` (`IdMulta`);

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
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=80;

--
-- AUTO_INCREMENT de la tabla `imagen`
--
ALTER TABLE `imagen`
  MODIFY `IdImagen` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=251;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `multa`
--
ALTER TABLE `multa`
  MODIFY `IdMulta` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=69;

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
-- Filtros para la tabla `multa`
--
ALTER TABLE `multa`
  ADD CONSTRAINT `multa_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`IdContrato`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_idusuarioanula` FOREIGN KEY (`IdUsuarioAnula`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `fk_pago_idusuariocrea` FOREIGN KEY (`IdUsuarioCrea`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`IdContrato`),
  ADD CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`IdContrato`),
  ADD CONSTRAINT `pago_ibfk_3` FOREIGN KEY (`IdUsuarioCrea`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `pago_ibfk_4` FOREIGN KEY (`IdUsuarioAnula`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `pago_ibfk_5` FOREIGN KEY (`IdMulta`) REFERENCES `multa` (`IdMulta`);

DELIMITER $$
--
-- Eventos
--
CREATE DEFINER=`root`@`localhost` EVENT `AnularContratosVencidos` ON SCHEDULE EVERY 1 DAY STARTS '2025-04-23 04:00:00' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
    UPDATE contrato
    SET EstadoContrato = 'Anulado',
        FechaUsuarioAnula = NOW()
    WHERE EstadoContrato = 'PendienteAnulacion'
      AND FechaRescision IS NOT NULL
      AND FechaRescision < CURDATE();
END$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
