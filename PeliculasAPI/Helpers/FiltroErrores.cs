using Microsoft.AspNetCore.Mvc.Filters;

namespace PeliculasAPI.Helpers
{
    public class FiltroErrores : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public FiltroErrores(ILogger logger)
        {
            this.logger = logger;
        }

        //con esto guardamos en el log local las excepciones que no pudieran ser atrapadas por un try/catch
        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
