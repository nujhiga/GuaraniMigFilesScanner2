namespace GuaraniMigFilesScanner.Class.Scanner.Issues
{
    public enum IssueType
    {
        None,

        Cantidad_Campos_Invalida,

        Cantidad_Caracteres_Invalida,

        Campo_Decimal_Invalido,

        Cantidad_Caracteres_Decimal_Entero_Invalida,

        Cantidad_Caracteres_Decimal_Decimal_Invalida,

        Campo_Vacio,

        CuitCuil_Invalido,

        Tipo_Dato_Invalido,

        Formato_Fecha_Invalido,

        Persona_Menor_de_15,

        Usuarios_Duplicados,

        CuitCuils_Duplicados,

        Documentos_Duplicados,

        Acta_Duplicada,

        Alumno_Duplicado,

        Docente_Legajos_Duplicados,

        Email_Invalido,

        Nacionalidad_Invalida,

        Titulo_Invalido,

        Titulo_Otro_Invalido,

        Institucion_Invalida,

        Institucion_Otra_Invalida,

        Pais_Documento_Invalido,

        Pais_Origen_Invalido,

        Tipo_Documento_Invalido,

        Fecha_Ingreso_Pais_Invalida,

        Colegio_Invalido,

        Colegio_Otro_Invalido,

        Localidad_Invalida,

        Localidad_Nombre_Invalida,

        Sexo_Invalido,

        Nro_Documento_Invalido,

        Nro_Documento_Inexistente,

        Periodo_Nombre_Inexistente,
        
        Actividad_Codigo_Inexistente,

        Elemento_Inexistente,

        Escala_Nota_Inexistente,

        Anios_Academico_Inexistente,

        Act_Estado,

        Act_Disponible_Para,

        Valor_Fuera_De_Rango,
        Persona_No_Existe,
        Alumno_No_Existe,
        Nro_Documento_Existente,
        Comision_Duplicada,
        Mesa_Examen_Existente,
        Mesa_Examen_No_Existe,
        Alumno_Existe,
        Plan_Propuesta_Invalido,
        Propuesta_Inexistente,
        Acta_Examen_Detalle_Duplicada,
        Acta_Examen_Duplicada,
        Acta_No_Existente,
        Acta_Existente,
        Nro_Libro_Existe,
        Nro_Libro_Inexistente,
        Nro_Libro_Duplicado
    }

    public enum IssueSeverity
    {
        None,
        Error,
        Warning
    }
}
