using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace DockerComposeReader
{
    public class Docker
    {
        public string? version { get; set; }
        public Dictionary<string, Service>? services { get; set; }
        public Dictionary<string, object>? volumes { get; set; }
        public Dictionary<string, object>? networks { get; set; }
        public Dictionary<string, object>? secrets { get; set; }
    }

    public class Service
    {
        public string? image { get; set; }
        public string? restart { get; set; }
        public Build? build { get; set; }
        public string[]? ports { get; set; }
        public string[]? volumes { get; set; }
        public string[]? networks { get; set;}
        public string? command { get; set; }
        public string[]? depends_on { get; set; }
        public string[]? env_file { get; set; }
        public string[]? secrets { get; set; }
        public dynamic? environment { get; set; }
        public object? healthcheck { get; set; }
        public string[]? extra_hosts { get; set; }
        public Extends? extends { get; set; }
    }

    public class Build
    {
        public string? context { get; set; }
        public string? dockerfile { get; set; }
        public object? args { get; set; }
    }

    public class Extends
    {
        public string? file { get; set; }
        public string? service { get; set; }
    }
    class Program
    {

        static void Main()
        {
            Console.WriteLine("Digite o path global do docker-compose.yml");
            string filePathDockerCompose =  Console.ReadLine()!.Trim('"')!;
            string yamlContent = File.ReadAllText(filePathDockerCompose);

            var deserializer = new DeserializerBuilder().Build();
            var serializedYaml = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                .Build();
            var docker = deserializer.Deserialize<Docker>(yamlContent);

            Console.WriteLine("Digite o path global do docker-compose.override.yml");
            string filePathDockerComposeOverride = Console.ReadLine()!.Trim('"')!;
            string yamlContentOverride = File.ReadAllText(filePathDockerComposeOverride);
            var dockerOverride = deserializer.Deserialize<Docker>(yamlContentOverride);
            Docker newDocker = new();
            Dictionary<string, Service> servicesInCompose = new();
            newDocker.version = $@"&{dockerOverride.version ?? docker.version}";
            newDocker.secrets = dockerOverride.secrets ?? docker.secrets;
            newDocker.volumes = dockerOverride.volumes ?? docker.volumes;
            newDocker.networks = dockerOverride.networks ?? docker.networks;
            foreach(var service in dockerOverride.services!)
            {
                Service serviceIn = new();
                docker.services!.TryGetValue(service.Key,out serviceIn!);
                if(serviceIn != null)
                {
                    servicesInCompose[service.Key] = service.Value;
                }
                else
                {
                    if(newDocker.services==null)
                    {
                        newDocker.services = new();
                    }
                    newDocker.services.Add(service.Key, service.Value);
                }
            }
            newDocker = Helpers.combineServices(newDocker, servicesInCompose);
            string newDockeString = serializedYaml.Serialize(newDocker).Replace("&", "");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string desktopFilePath = Path.Combine(desktopPath, "docker-compose.devcontainer.yml");

            try
            {
                File.WriteAllText(desktopFilePath, newDockeString);
                Console.WriteLine($"File saved to desktop: {desktopFilePath}");
            }
            catch (Exception desktopEx)
            {
                Console.WriteLine($"Error saving file to desktop: {desktopEx.Message}");
            }
            Console.ReadLine();
        }
        
    }

    public static class Helpers
    {
        public static Docker combineServices(Docker docker, Dictionary<string, Service> services)
        {
            Random random = new Random();
            Service newService = new();
            newService.image = $"{random.Next(0, 1000)}_devcontainer";
            newService.build = new Build() { context = ".", dockerfile = "Dockerfile" };
            newService.command = "sleep infinity";
            List<string> volumes = new List<string>();
            List<string> ports = new List<string>();
            List<string> environmensts = new List<string>();
            List<string> networks = new List<string>();
            List<string> env_files = new List<string>();
            List<string> secrets = new List<string>();
            List<string> extra_hosts = new List<string>();
            volumes.Add("../..:/workspaces:cached");
            foreach (var service in services)
            {
                var s = service.Value;

                //Add ports
                if (s.ports != null)
                {
                    foreach (var port in s.ports)
                    {
                        bool add = true;
                        foreach (var item in ports)
                        {
                            if (item.Equals($@"&{port}"))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            ports.Add($@"&{port}");
                        }
                    }
                }


                //Add Environments
                if (s.environment != null)
                {

                    try
                    {
                            foreach (var env in s.environment)
                            {
                                bool add = true;
                                var envString = $"{env.Key}={env.Value}";
                                foreach (var item in environmensts)
                                {
                                    if (item.Equals(envString))
                                    {
                                        add = false;
                                    }
                                }
                                if (add)
                                {
                                    environmensts.Add(envString);
                                }
                            }
                        
                    }
                    catch
                    {
                        foreach (var env in s.environment)
                        {
                            bool add = true;
                            foreach (var item in environmensts)
                            {
                                if (item.Equals(env))
                                {
                                    add = false;
                                }
                            }
                            if (add)
                            {
                                environmensts.Add(env);
                            }
                        }
                    }
                }


                //Add Networks
                if(s.networks != null)
                {
                    foreach (var net in s.networks)
                    {
                        bool add = true;
                        foreach (var item in networks)
                        {
                            if (item.Equals(net))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            networks.Add(net);
                        }
                    }
                }


                //Add Env files
                if (s.env_file != null)
                {
                    foreach (var envFile in s.env_file)
                    {
                        bool add = true;
                        foreach (var item in env_files)
                        {
                            if (item.Equals(envFile))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            env_files.Add(envFile);
                        }
                    }
                }


                //Add secrets
                if (s.secrets != null)
                {
                    foreach (var secret in s.secrets)
                    {
                        bool add = true;
                        foreach (var item in secrets)
                        {
                            if (item.Equals(secret))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            secrets.Add(secret);
                        }
                    }
                }


                //Add extra_hosts
                if (s.extra_hosts != null)
                {
                    foreach (var extra_host in s.extra_hosts)
                    {
                        bool add = true;
                        foreach (var item in extra_hosts)
                        {
                            if (item.Equals(extra_host))
                            {
                                add = false;
                            }
                        }
                        if (add)
                        {
                            extra_hosts.Add(extra_host);
                        }
                    }
                }
            }
            newService.volumes = volumes.Any()? volumes.ToArray(): null;
            newService.ports = ports.Any() ? ports.ToArray() : null;
            newService.networks = networks.Any() ? networks.ToArray() : null;
            newService.environment = environmensts.Any() ? environmensts.ToArray() : null;
            newService.env_file = env_files.Any() ? env_files.ToArray() : null;
            newService.secrets = secrets.Any() ? secrets.ToArray() : null;
            newService.extra_hosts = extra_hosts.Any() ? extra_hosts.ToArray() : null;
            docker.services?.Add("app",newService);
            return docker;
        }
    }
}
