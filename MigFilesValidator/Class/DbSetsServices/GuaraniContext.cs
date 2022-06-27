using PasifaeG3Migrations.Class.DbSetsServices.DbModels.G3Models;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuaraniMigFilesScanner;
using GuaraniMigFilesScanner.Class.Connections;

namespace PasifaeG3Migrations.Class.DbSetsServices
{
    public class GuaraniContext
    {


        public Repository Repository { get; }
        public ConcurrentDictionary<object, PropuestaElementos> PropuestaElementos { get; private set; }
        public ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>> ActasRegulares { get; private set; }
        public ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>> ActasPromocion { get; private set; }
        public ConcurrentDictionary<object, ActaExamen> ActasExamenes { get; private set; }
        public ConcurrentDictionary<object, MesaExamen> MesasExamen { get; private set; }
        public ConcurrentDictionary<object, Persona> Personas { get; private set; }
        public ConcurrentDictionary<object, Alumno> Alumnos { get; private set; }
        //  public ConcurrentDictionary<string, int> Periodos { get; private set; }
        //  public ConcurrentDictionary<string, string> Elementos { get; private set; }
        //  public ConcurrentDictionary<string, int> MesasExamenNombres { get; private set; }
        //  public ConcurrentDictionary<string, int> EscalasNotas { get; private set; }
        //  public ConcurrentDictionary<object, AnioAcademico> AniosAcademicos { get; private set; }  
        public ConcurrentDictionary<object, LibroActas> LibrosActas { get; private set; }
        public ConcurrentDictionary<object, EscalaNota> EscalasNotas { get; private set; }
        public List<Propuesta> Propuestas { get; private set; }
        public List<Elemento> Elementos { get; private set; }
        public IEnumerable<AnioAcademico> AniosAcademicos { get; private set; }
        public ConcurrentDictionary<object, Periodo> Periodos { get; private set; }

        public GuaraniContext(Connection conn) => Repository = new Repository(conn);

        public async Task InitPropuestaElementos() => PropuestaElementos = await Repository.GetPropuestasElementosAsync();
        public async Task InitPersonas() => Personas = await Repository.GetPersonasAsync();
        public async Task InitAlumnos() => Alumnos = await Repository.GetAlumnosAsync();
        public async Task InitActasExamenes() => ActasExamenes = await Repository.GetActasExamenesAsync();
        public async Task InitActasCursadaRegulares() => ActasRegulares = await Repository.GetActasRegularesAsync();
        public async Task InitActasCursadaPromocion() => ActasPromocion = await Repository.GetActasPromocionAsync();
        public async Task InitPropuestas() => Propuestas = await Repository.GetPropuestasAsync();
        public async Task InitElementos() => Elementos = await Repository.GetElementosAsync();
        public async Task InitAniosAcademicos() => AniosAcademicos = await Repository.GetAniosAcademicosAsync();
        public async Task InitLibrosActas() => LibrosActas = await Repository.GetLibrosActasAsync();
        public async Task InitPeriodos() => Periodos = await Repository.GetPeriodosAsync();
        public async Task InitEscalasNotas() => EscalasNotas = await Repository.GetEscalasNotasAsync();
        public async Task InitMesasExamen() => MesasExamen = await Repository.GetMesasExamenAsync();
        
        //public Repository Repository { get; }
        //public ConcurrentDictionary<object, PropuestaElementos> PropuestaElementos { get; private set; }
        //public ConcurrentDictionary<object, ActaCursada<ActaRegularesDetalle>> ActasRegulares { get; private set; }
        //public ConcurrentDictionary<object, ActaCursada<ActaRegularPromocionDetalle>> ActasPromocion { get; private set; }
        //public ConcurrentDictionary<object, ActaExamen> ActasExamenes { get; private set; }
        //public ConcurrentDictionary<object, Persona> Personas { get; private set; }
        //public ConcurrentDictionary<object, Alumno> Alumnos { get; private set; }
        //public ConcurrentDictionary<string, int> Periodos { get; private set; }
        //public ConcurrentDictionary<string, string> Elementos { get; private set; }
        //public ConcurrentDictionary<string, int> MesasExamenNombres { get; private set; }
        //public ConcurrentDictionary<string, int> EscalasNotas { get; private set; }

        //public List<Propuesta> Propuestas { get; private set; }
        //public GuaraniContext(Connection conn) => Repository = new Repository(conn);
        //public async Task InitPropuestaElementos() => PropuestaElementos = await Repository.GetPropuestasElementosAsync();
        //public async Task InitPersonas() => Personas = await Repository.GetPersonasAsync();
        //public async Task InitAlumnos() => Alumnos = await Repository.GetAlumnosAsync();
        //public async Task InitActasExamenes() => ActasExamenes = await Repository.GetActasExamenesAsync();
        //public async Task InitActasCursadaRegulares() => ActasRegulares = await Repository.GetActasRegularesAsync();
        //public async Task InitActasCursadaPromocion() => ActasPromocion = await Repository.GetActasPromocionAsync();
        //public async Task InitElementos() => Elementos = await Repository.GetSgaElementosCodes();
        //public async Task InitPeriodos() => Periodos = await Repository.GetSgaPeriodosNames();
        //public async Task InitEscalasNotas() => EscalasNotas = await Repository.GetSgaEscalasNotas();
        //public async Task InitMesas() => MesasExamenNombres = await Repository.GetSgaMesasExamen();
        //public async Task InitPropuestas() => Propuestas = await Repository.GetPropuestasAsync();


    }
}
