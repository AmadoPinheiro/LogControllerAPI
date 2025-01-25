using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using LogTransformer.Api.Models;
using LogTransformer.Core.Entities;
using LogTransformer.Core.Interfaces;
using LogTransformer.Core.Services;
using LogTransformer.Core.Validations;
using Microsoft.AspNetCore.Mvc;

namespace LogTransformer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly LogTransformerService _logTransformerService;
        private readonly IMapper _mapper;
        public LogsController(ILogRepository logRepository, LogTransformerService logTransformerService, IMapper mapper)
        {
            _logRepository = logRepository;
            _logTransformerService = logTransformerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] LogEntryDto logDto)
        {
            try
            {

                if (logDto == null)
                {
                    return BadRequest("Log não pode ser nulo.");
                }
                var log = _mapper.Map<LogEntry>(logDto);

                LogValidator.EnsureValidLog(log.OriginalLog);
                await _logRepository.AddLogAsync(log);
                return Ok("Log salvo com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest("ERROR" + ex);
            }
        }

        [HttpGet("transform/{id}")]
        public async Task<IActionResult> TransformLogWithOptions(int id, [FromQuery] bool saveToFile = false)
        {
            var log = await _logRepository.GetLogByIdAsync(id);
            if (log == null)
            {
                return NotFound("Log não encontrado.");
            }

            var transformedLog = _logTransformerService.TransformLog(log.OriginalLog);
            var logsWithHeader = _logTransformerService.AddHeaderToLogs(new List<string> { transformedLog });

            if (saveToFile)
            {
                var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                if (!Directory.Exists(logsDirectory))
                {
                    Console.WriteLine("Criando diretório de logs: " + logsDirectory);
                    Directory.CreateDirectory(logsDirectory);
                }

                string filePath = Path.Combine(logsDirectory, $"log_{id}_transformed.txt");

                try
                {
                    await System.IO.File.WriteAllTextAsync(filePath, transformedLog);

                    if (!System.IO.File.Exists(filePath))
                    {
                        return StatusCode(500, "Erro ao criar o arquivo de log transformado.");
                    }

                    Console.WriteLine($"Arquivo de log transformado salvo em: {filePath}");

                    return Ok(new { FilePath = filePath });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao salvar o log transformado: {ex.Message}");
                }
            }

            return Ok(new { TransformedLog = logsWithHeader });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            var logs = await _logRepository.GetAllLogsAsync();

            var transformedLogs = logs.Select(log => new
            {
                log.OriginalLog,
                TransformedLog = _logTransformerService.TransformLog(log.OriginalLog)
            });

            return Ok(transformedLogs);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogById(int id)
        {
            try
            {
                var log = await _logRepository.GetLogByIdAsync(id);
                if (log == null)
                {
                    return NotFound("Log não encontrado.");
                }
                var logResponse = _mapper.Map<LogResponseDto>(log);

                logResponse.TransformedLog = _logTransformerService.TransformLog(log.OriginalLog);

                return Ok(logResponse);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar log" + ex);
            }
        }

        [HttpGet("transformed")]
        public async Task<IActionResult> GetTransformedLogs()
        {
            var logs = await _logRepository.GetAllLogsAsync();

            var transformedLogs = logs
                .Select(log => _logTransformerService.TransformLog(log.OriginalLog))
                .Where(transformedLog => !string.IsNullOrEmpty(transformedLog))
                .ToList();

            var logsWithHeader = _logTransformerService.AddHeaderToLogs(transformedLogs);

            return Ok(new { TransformedLogs = logsWithHeader });
        }
        [HttpPost("transform")]
        public async Task<IActionResult> TransformLogs([FromBody] Newtonsoft.Json.Linq.JToken input, [FromQuery] bool saveToFile = false)
        {
            try
            {
                List<string> logs = new List<string>();

                if (input.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    logs = input.ToObject<List<string>>();
                }
                else if (input.Type == Newtonsoft.Json.Linq.JTokenType.String)
                {
                    var url = input.ToString();

                    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        return BadRequest("A entrada deve ser uma lista de logs ou uma URL válida.");
                    }

                    using (var client = new HttpClient())
                    {
                        var logContent = await client.GetStringAsync(url);
                        logs = logContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                }
                else
                {
                    return BadRequest("A entrada deve ser uma lista de logs ou uma URL válida.");
                }

                if (!logs.Any())
                {
                    return BadRequest("A lista de logs não pode ser vazia.");
                }

                var transformedLogs = logs.Select(log => _logTransformerService.TransformLogAgoraFormat(log)).ToList();
                var logsWithHeader = _logTransformerService.AddHeaderToLogs(transformedLogs);

                if (saveToFile)
                {
                    var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                    if (!Directory.Exists(logsDirectory))
                    {
                        Directory.CreateDirectory(logsDirectory);
                    }

                    string filePath = Path.Combine(logsDirectory, $"logs_transformed_{DateTime.UtcNow:yyyyMMddHHmmss}.txt");

                    await System.IO.File.WriteAllTextAsync(filePath, logsWithHeader);

                    return Ok(new { FilePath = filePath });
                }

                return Ok(new { TransformedLogs = logsWithHeader });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar os logs: {ex.Message}");
            }
        }


    }
}
