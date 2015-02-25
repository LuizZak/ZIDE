namespace ZIDE.Controllers
{
    /// <summary>
    /// Controls project management on the interface
    /// </summary>
    public class ProjectsController
    {
        /// <summary>
        /// The main controller this projects controller is binded to
        /// </summary>
        private readonly MainController _mainController;

        /// <summary>
        /// Gets the main controller this projects controller is binded to
        /// </summary>
        public MainController MainController
        {
            get { return _mainController; }
        }

        /// <summary>
        /// Initializes a new instance of the ProjectsController class
        /// </summary>
        /// <param name="mainController">The main controller to bind this projects controller to</param>
        public ProjectsController(MainController mainController)
        {
            _mainController = mainController;
        }
    }
}