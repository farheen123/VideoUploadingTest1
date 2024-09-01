using Microsoft.AspNetCore.Mvc;
using VideoUploadingTest.Services.Interface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly string _mediaFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServerMediaFiles");
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private const long _maxFileSize = 100 * 1028 * 1028;
    public HomeController(IOptions<FormOptions> formOptions,ILogger<HomeController> logger, IConfiguration configuration, IFileService fileService)
    {
        _logger = logger;
        _configuration = configuration;
        _fileService = fileService;
    }
    public IActionResult Index()
    {
        var files = _fileService.GetFiles();
        return View(files);
    }
    [HttpPost]
    public async Task<ActionResult> Index(IFormFile[] files)
    {

        List<string> errorMessages = new List<string>();
        List<string> successMessages = new List<string>();

        if (files == null || !files.Any())
        {
            errorMessages.Add("No files uploaded.");
            ViewBag.ErrorMessages = errorMessages;
            return View();
        }
        else
        {
            foreach (var file in files)
            {
                if (file.Length > _maxFileSize)
                {
                    errorMessages.Add($"File '{file.FileName}' exceeds the 100 MB limit.");
                }
                else
                {
                    try
                    {
                        if (await _fileService.UploadFile(file))
                        {
                            successMessages.Add($"File '{file.FileName}' Upload Successful.");

                            //ViewBag.SuccessMessages = "File Upload Successful";
                        }
                        else
                        {
                            errorMessages.Add($"File '{file.FileName}' Upload Failed");
                            // ViewBag.Error = "File Upload Failed";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log ex
                        errorMessages.Add($"File Upload Failed");
                        //ViewBag.Error = "File Upload Failed";
                    }

                }

                // Additional check for empty files
                if (file.Length == 0)
                {
                    errorMessages.Add($"File '{file.FileName}' is empty.");
                }
            }
            ViewBag.ErrorMessages = errorMessages;
            ViewBag.SuccessMessages = successMessages;
            return View();
        }
    }
}


