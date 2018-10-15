﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using JetBrains.Annotations;
using RunMode = BenchmarkDotNet.Diagnosers.RunMode;

namespace BenchmarkDotNet.Configs
{
    public static class ConfigExtensions
    {
        [PublicAPI] public static IConfig With(this IConfig config, params IColumn[] columns) => config.With(m => m.Add(columns));
        [PublicAPI] public static IConfig With(this IConfig config, params IColumnProvider[] columnProviders) => config.With(m => m.Add(columnProviders));
        [PublicAPI] public static IConfig With(this IConfig config, params ILogger[] loggers) => config.With(m => m.Add(loggers));
        [PublicAPI] public static IConfig With(this IConfig config, params IExporter[] exporters) => config.With(m => m.Add(exporters));
        [PublicAPI] public static IConfig With(this IConfig config, params IDiagnoser[] diagnosers) => config.With(m => m.Add(diagnosers));
        [PublicAPI] public static IConfig With(this IConfig config, params IAnalyser[] analysers) => config.With(m => m.Add(analysers));
        [PublicAPI] public static IConfig With(this IConfig config, params IValidator[] validators) => config.With(m => m.Add(validators));
        [PublicAPI] public static IConfig With(this IConfig config, params Job[] jobs) => config.With(m => m.Add(jobs));
        [PublicAPI] public static IConfig With(this IConfig config, IOrderer provider) => config.With(m => m.Orderer = provider);
        [PublicAPI] public static IConfig With(this IConfig config, params HardwareCounter[] counters) => config.With(c => c.Add(counters));
        [PublicAPI] public static IConfig With(this IConfig config, params IFilter[] filters) => config.With(c => c.Add(filters));
        [PublicAPI] public static IConfig With(this IConfig config, Encoding encoding) => config.With(c => c.Set(encoding));
        [PublicAPI] public static IConfig With(this IConfig config, ISummaryStyle summaryStyle) => config.With(c => c.SummaryStyle = summaryStyle);

        /// <summary>
        /// determines if all auto-generated files should be kept or removed after running the benchmarks
        /// </summary>
        [PublicAPI] public static IConfig KeepBenchmarkFiles(this IConfig config, bool value = true) => config.With(m => m.KeepBenchmarkFiles = value);
        [PublicAPI] public static IConfig RemoveBenchmarkFiles(this IConfig config) => config.KeepBenchmarkFiles(false);
        [PublicAPI] public static IConfig WithArtifactsPath(this IConfig config, string artifactsPath) => config.With(m => m.ArtifactsPath = artifactsPath);
        [PublicAPI] public static IConfig With(this IConfig config, params BenchmarkLogicalGroupRule[] rules) => config.With(c => c.Add(rules));

        public static bool HasMemoryDiagnoser(this IConfig config) => config.GetDiagnosers().Any(diagnoser => diagnoser is MemoryDiagnoser);

        public static FinalConfig CreateFinalConfig(this IConfig config) => FinalConfigBuilder.Create(config);

        private static IConfig With(this IConfig config, Action<ManualConfig> addAction)
        {
            var manualConfig = ManualConfig.Create(config);
            addAction(manualConfig);
            return manualConfig;
        }
    }
}