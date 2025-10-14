SELECT count(*) as cotizados, sum(Servicio_realizado) as confirmados, CAST(sum(Servicio_realizado) as REAL)/count(*)*100 as porcentaje, b.Nombre
FROM Cotizacion  a INNER JOIN Operador b on a.Id_Operador = b.Id
WHERE DATE(Fecha) BETWEEN '2021-09-20' AND '2021-09-22'
GROUP by Id_Operador

SELECT b. Nombre, Cliente from Cotizacion a INNER JOIN Operador b on a.Id_Operador = b.Id

INSERT INTO Operador (Nombre) VALUES ('Adrian')