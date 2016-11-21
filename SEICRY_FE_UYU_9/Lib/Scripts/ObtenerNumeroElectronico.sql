create procedure ObtenerNumeroElectronico
@tipo varchar(25)
as

Declare
@numeroActual int,
@idRango nvarchar,
@numeroFinal int,
@numeroInicial int

--Obtener número siguiente del rango activo para un tipo específico de documento
set nocount on;
select @idRango = T1.U_IdCAE, @numeroActual = U_NumAct 
from [@TFERANGO] as T1
inner join [@TFECAE] as T2
on T1.U_IdCAE = T2.DocEntry
where T1.U_TipoDoc =  @tipo
and T2.U_Sucursal = @sucursal
--and T2.U_Caja = @caja
and U_Activo = 'Y'

--Validar que aún hayan números disponibles. Si no hay se retorna -1
if (@numeroActual is null) 
begin
	select '-1', '-' from [SBODemoES].[dbo].[@TFERANGO] 
	return
end

--Si aún quedan número disponibles valida que estén dentro de las fechas y que el último retornado sea menor que el último disponible
else
	
	--Obtener el DocEntry del rango vigente para el tipo de documento ingresado
	select @idRango = DocEntry
	from [dbo].[@TFERANGO] 
	where U_TipoDoc =  @tipo
	and CONVERT(date, GETDATE()) >= CONVERT(date, U_ValDesde) and CONVERT(date, GETDATE()) <= CONVERT(date, U_ValHasta)
	
	--Valida que exista un rango vigente para el tipo de documento. Si no lo hay inactiva el rango y retorna -1
	if @idRango is null
	begin
		
		update [dbo].[@TFERANGO]  set U_Activo = 'N' where U_TipoDoc = @tipo and U_Activo = 'Y'
		
		select '-1', '-' from [dbo].[@TFERANGO] 
		return
	end
	else
	
	--Si aún el rango tiene números disponibles y el rango está vigente, se actualiza el siguiente valor
	update [dbo].[@TFERANGO] set U_NumAct = ( @numeroActual + 1 ) where U_TipoDoc = @tipo
	
	--Valida que el siguiente numero sea menor o igual al numero final del rango
	select @idRango = DocEntry, @numeroActual = U_NumAct, @numeroInicial = U_NumIni, @numeroFinal = U_NumFin
	from [dbo].[@TFERANGO] 
	where U_TipoDoc =  @tipo
	and U_Activo = 'Y'
	
	--Si los números siguiente y final son iguales se inactiva el rango
	if @numeroActual = @numeroFinal
	begin
		update [dbo].[@TFERANGO]  set U_Activo = 'N' where DocEntry = @idRango
	end
	
	--Si el numero siguiente es menor que el final es retornado
	select U_NumAct, U_Serie from [dbo].[@TFERANGO] where DocEntry = @idRango
go

