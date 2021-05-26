using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace WebApplication1
{
    [Route("api")]
    public class FileController : Controller
    {
        private readonly string _path = "C:\\WebApplication1\\WebApplication1\\wwwroot\\Files\\";

        [HttpPut("PUT")]
        public IActionResult InputFile([FromForm] InputFile input)
        {
            try
            {
                //path/hello/newpath/file.pdf
                new System.IO.FileInfo(_path + input.Name).Directory.Create();
                using (var fileStream = new FileStream(Path.Combine(_path, input.Name), FileMode.Create))
                {
                    input.File.CopyTo(fileStream);
                }
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GET")]
        public IActionResult GetFile(string namefile)
        {
            if (System.IO.File.Exists(Path.Combine(_path, namefile)))
            {
                try
                {
                    string path = Path.Combine(_path, namefile);
                    FileStream file = new FileStream(path, FileMode.Open);
                    return File(file, "application/unknown", Path.GetFileName(namefile));
                }
                catch
                {
                    return NotFound();
                }
            }
            else
            {
                string directoryname = namefile;
                try
                {
                    IReadOnlyCollection<string> files = FileSystem.GetFiles(Path.Combine(_path, directoryname));
                    IReadOnlyCollection<string> directories = FileSystem.GetDirectories(Path.Combine(_path, directoryname));

                    List<FileEl> content = new List<FileEl>();

                    foreach (var item in directories)
                    {
                        content.Add(new FileEl(Path.GetFileName(item), "Папка"));
                    }
                    foreach (var item in files)
                    {
                        content.Add(new FileEl(Path.GetFileName(item), "Файл"));
                    }
                    return new JsonResult(content, new JsonSerializerOptions { });
                }
                catch
                {
                    return NotFound();
                }

            }
        }

        [HttpGet("INFO")]
        public IActionResult GetInfo(string namefile)
        {
            string path = Path.Combine(_path, namefile);
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                infoFile info = new infoFile();
                info.Name = fileInfo.Name;
                info.Path = fileInfo.DirectoryName;
                info.Size = fileInfo.Length;
                info.CreationDate = fileInfo.CreationTime.ToString();
                info.ChangedDate = fileInfo.LastWriteTime.ToString();
                return Ok(info);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("DELETE")]
        public IActionResult DeleteFile(string namefile)
        {
            if (System.IO.File.Exists(Path.Combine(_path, namefile)))
            {
                try
                {
                    FileSystem.DeleteFile(Path.Combine(_path, namefile));
                    return Ok();
                }
                catch
                {
                    return NotFound();
                }
            }
            else
            {
                try
                {
                    FileSystem.DeleteDirectory(Path.Combine(_path, namefile), DeleteDirectoryOption.DeleteAllContents);
                    return Ok();
                }
                catch
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("X-Copy-From")]
        public IActionResult CopyFrom(CopyFileInput input)
        {
            string path = _path + input.FromFolder + "\\" + input.FileName;
            new System.IO.FileInfo(_path + input.ToFolder).Directory.Create();
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                try
                {
                    string destFileName = _path + input.ToFolder + "\\" + input.FileName;

                    System.IO.File.Copy(path, destFileName);
                }
                catch
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
