global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Text.Json;
global using Microsoft.EntityFrameworkCore;
global using Confluent.Kafka;
global using Manonero.MessageBus.Kafka.Abstractions;
global using static Confluent.Kafka.ConfigPropertyNames;

global using Kafka_Example_01_API.BackgroundTasks;
global using Kafka_Example_01_API.Commands.CommandModels;
global using Kafka_Example_01_API.Commands.Handlers;
global using Kafka_Example_01_API.Extensions;
global using Kafka_Example_01_API.Models.Requests;
global using Kafka_Example_01_API.Producers;
global using Kafka_Example_01_API.Repositories;
global using Kafka_Example_01_API.Services;
global using Kafka_Example_01_API.Settings;

global using Kafka_Example_01_API.Core;
global using Kafka_Example_01_API.Core.DTOs;
global using Kafka_Example_01_API.Core.InMemories;
global using Kafka_Example_01_API.Core.Interfaces.Commands;
global using Kafka_Example_01_API.Core.IRepositories;
global using Kafka_Example_01_API.Core.IServices;
global using Kafka_Example_01_API.Core.Models;