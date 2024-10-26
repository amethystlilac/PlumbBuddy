namespace PlumbBuddy.Services.Scans;

public sealed class DependencyScan :
    Scan,
    IDependencyScan
{
    [Flags]
    enum MissingDependencyModResolutionType :
        int
    {
        CompleteMetadata = 0,
        UnnamedDependent = 0x1,
        UnnamedDependency = 0x2,
        IdenticallyNamed = 0x4,
        UnspecifiedDependentSource = 0x8,
        UnspecifiedDependencySource = 0x10,

        Normal = CompleteMetadata,
        FileNeedsDependency = UnnamedDependent,
        ModNeedsDownload = UnnamedDependency,
        FileNeedsDownload = UnnamedDependent | UnnamedDependency,
        ReinstallMod = UnspecifiedDependencySource,
        ReinstallFile = UnspecifiedDependencySource | UnnamedDependent,
        BrokenFile = UnspecifiedDependentSource | UnspecifiedDependencySource
    }

    record ModWithIncompatiblePacks(string Name, IReadOnlyList<string> IncompatiblePackCodes, IReadOnlyList<string> FilePaths);
    record ModWithMissingDependencyMod(long ModManifestId, string? RequirementIdentifier, int CommonRequirementIdentifiers, string? Name, IReadOnlyList<string> Creators, Uri? Url, string? DependencyName, IReadOnlyList<string> DependencyCreators, Uri? DependencyUrl, IReadOnlyList<string> FilePaths, bool WasFeatureRemoved);
    record ModWithMissingPacks(string Name, IReadOnlyList<string> Creators, string? ElectronicArtsPromoCode, IReadOnlyList<string> MissingPackCodes, IReadOnlyList<string> FilePaths);

    public DependencyScan(IPlatformFunctions platformFunctions, IBlazorFramework blazorFramework, IPlayer player, ISmartSimObserver smartSimObserver, PbDbContext pbDbContext)
    {
        ArgumentNullException.ThrowIfNull(platformFunctions);
        ArgumentNullException.ThrowIfNull(blazorFramework);
        ArgumentNullException.ThrowIfNull(player);
        ArgumentNullException.ThrowIfNull(smartSimObserver);
        ArgumentNullException.ThrowIfNull(pbDbContext);
        this.platformFunctions = platformFunctions;
        this.blazorFramework = blazorFramework;
        this.player = player;
        this.smartSimObserver = smartSimObserver;
        this.pbDbContext = pbDbContext;
    }

    readonly IBlazorFramework blazorFramework;
    readonly PbDbContext pbDbContext;
    readonly IPlatformFunctions platformFunctions;
    readonly IPlayer player;
    readonly ISmartSimObserver smartSimObserver;

    public override async Task ResolveIssueAsync(object issueData, object resolutionData)
    {
        if (resolutionData is string resolutionStr)
        {
            if (issueData is ModWithMissingPacks modWithMissingPacks && resolutionStr.StartsWith("purchase-"))
                await smartSimObserver.HelpWithPackPurchaseAsync(resolutionStr[9..], blazorFramework.MainLayoutLifetimeScope!.Resolve<IDialogService>(), modWithMissingPacks.Creators, modWithMissingPacks.ElectronicArtsPromoCode).ConfigureAwait(false);
            else if (resolutionStr.StartsWith("showfile-") && new FileInfo(Path.Combine(player.UserDataFolderPath, "Mods", resolutionStr[9..])) is { } modFile && modFile.Exists)
                platformFunctions.ViewFile(modFile);
        }
    }

    public override async IAsyncEnumerable<ScanIssue> ScanAsync()
    {
        var installedPackCodes = smartSimObserver.InstalledPackCodes;
        await foreach (var modWithMissingPacks in pbDbContext.ModFileManifests
            .Where(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null) && mfm.RequiredPacks!.Any(pc => !installedPackCodes.Contains(pc.Code.ToUpper())))
            .Select(mfm => new ModWithMissingPacks
            (
                mfm.Name,
                mfm.Creators!.Select(c => c.Name).ToList(),
                mfm.ElectronicArtsPromoCode == null ? null : mfm.ElectronicArtsPromoCode.Code,
                mfm.RequiredPacks!.Where(pc => !installedPackCodes.Contains(pc.Code.ToUpper())).Select(pc => pc.Code.ToUpper()).ToList(),
                mfm.ModFileHash!.ModFiles!.Where(mf => mf.Path != null && mf.AbsenceNoticed == null).Select(mf => mf.Path!).ToList()
            ))
            .AsAsyncEnumerable())
        {
            yield return new ScanIssue
            {
                Caption = $"{(string.IsNullOrWhiteSpace(modWithMissingPacks.Name) ? "A Mod" : modWithMissingPacks.Name)} is Missing {"Pack".ToQuantity(modWithMissingPacks.MissingPackCodes.Count, ShowQuantityAs.Words)}",
                Description =
                    $"""
                    **{(string.IsNullOrWhiteSpace(modWithMissingPacks.Name) ? "A Mod" : modWithMissingPacks.Name)}** requires **{modWithMissingPacks.MissingPackCodes.Humanize()}**, which {(modWithMissingPacks.MissingPackCodes.Count is 1 ? "is" : "are")} purchasable downloadable content for The Sims 4 from Electronic Arts which you do not have installed.<br />
                    *If you believe you purchased this content already, you may want to check for any The Sims 4 DLC you may have available for download in {(smartSimObserver.IsSteamInstallation ? "Steam" : "the EA app")}*.
                    """,
                Icon = MaterialDesignIcons.Normal.BagPersonalOff,
                Type = ScanIssueType.Sick,
                Origin = this,
                Data = modWithMissingPacks,
                Resolutions =
                [
                    ..modWithMissingPacks.MissingPackCodes.Select(missingPackCode => new ScanIssueResolution
                    {
                        Label = $"Help me Purchase {missingPackCode}",
                        Icon = MaterialDesignIcons.Normal.Store,
                        Color = MudBlazor.Color.Primary,
                        Data = $"purchase-{missingPackCode}"
                    }),
                    ..modWithMissingPacks.FilePaths.Select(filePath => new ScanIssueResolution
                    {
                        Label = $"Show me the file for {(string.IsNullOrWhiteSpace(modWithMissingPacks.Name) ? "the mod" : modWithMissingPacks.Name)}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Default,
                        Data = $"showfile-{filePath}"
                    })
                ]
            };
        }
        await foreach (var modWithIncompatiblePacks in pbDbContext.ModFileManifests
            .Where(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null) && mfm.IncompatiblePacks!.Any(pc => installedPackCodes.Contains(pc.Code.ToUpper())))
            .Select(mfm => new ModWithIncompatiblePacks
            (
                mfm.Name,
                mfm.IncompatiblePacks!.Where(pc => installedPackCodes.Contains(pc.Code.ToUpper())).Select(pc => pc.Code.ToUpper()).ToList(),
                mfm.ModFileHash!.ModFiles!.Where(mf => mf.Path != null && mf.AbsenceNoticed == null).Select(mf => mf.Path!).ToList()
            ))
            .AsAsyncEnumerable())
            yield return new ScanIssue
            {
                Caption = $"{(string.IsNullOrWhiteSpace(modWithIncompatiblePacks.Name) ? "A Mod" : modWithIncompatiblePacks.Name)} is Incompatible with {"Installed Pack".ToQuantity(modWithIncompatiblePacks.IncompatiblePackCodes.Count, ShowQuantityAs.Words)}",
                Description =
                    $"""
                    **{(string.IsNullOrWhiteSpace(modWithIncompatiblePacks.Name) ? "A Mod" : modWithIncompatiblePacks.Name)}** is incompatible with **{modWithIncompatiblePacks.IncompatiblePackCodes.Humanize()}**, which {(modWithIncompatiblePacks.IncompatiblePackCodes.Count is 1 ? "is" : "are")} purchasable downloadable content for The Sims 4 from Electronic Arts which you have installed.
                    """,
                Icon = MaterialDesignIcons.Normal.BagPersonalTag,
                Type = ScanIssueType.Sick,
                Origin = this,
                Data = modWithIncompatiblePacks,
                Resolutions =
                [
                    new()
                    {
                        Label = $"Help me Disable or Remove Packs",
                        Icon = MaterialDesignIcons.Normal.BagPersonalOff,
                        Color = MudBlazor.Color.Primary,
                        Data = $"remove-packs",
                        Url = new("https://jamesturner.yt/disablepacks", UriKind.Absolute)
                    },
                    ..modWithIncompatiblePacks.FilePaths.Select(filePath => new ScanIssueResolution
                    {
                        Label = $"Show me the file for {(string.IsNullOrWhiteSpace(modWithIncompatiblePacks.Name) ? "the mod" : modWithIncompatiblePacks.Name)}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Default,
                        Data = $"showfile-{filePath}"
                    })
                ]
            };
        var modsWithMissingDependencyMod = new List<ModWithMissingDependencyMod>();
        var commonRequirementIdentifiers = new Dictionary<(long modManifestId, string requirementIdentifier), List<ModWithMissingDependencyMod>>();
        await foreach (var modWithMissingDependencyMod in pbDbContext.RequiredMods
            .Where(rm =>
                rm.ModFileManifest!.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null) // mod is present in Mods folder
                && (rm.IgnoreIfHashAvailable == null // ignore if hash available is unset
                    || !rm.IgnoreIfHashAvailable.ManifestsByCalculation!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null)) // hash is not available by calculation
                    && !rm.IgnoreIfHashAvailable.ManifestsBySubsumption!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null))) // hash is not available by subsumption
                && (rm.IgnoreIfHashUnavailable == null // ignore if hash unavailable is unset
                    || rm.IgnoreIfHashUnavailable.ManifestsByCalculation!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null)) // hash is available by calculation
                    || rm.IgnoreIfHashUnavailable.ManifestsBySubsumption!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null))) // hash is available by subsumption
                && (rm.IgnoreIfPackAvailable == null // ignore if pack available is unset
                    || !installedPackCodes.Contains(rm.IgnoreIfPackAvailable.Code)) // pack is not installed
                && (rm.IgnoreIfPackUnavailable == null // ignore if pack unavailable is unset
                    || installedPackCodes.Contains(rm.IgnoreIfPackUnavailable.Code)) // pack is installed
                && (rm.Hashes!.Any(h => // at least one required hash is
                    !h.ManifestsByCalculation!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null)) //... not available via calculation
                        && !h.ManifestsBySubsumption!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null))) // and not available via subsumption
                    || rm.RequiredFeatures!.Any(rf => !rm.Hashes!.Any(h => // ... or at least one feature is not
                        h.ManifestsByCalculation!.Any(mfm => mfm.Features!.Any(f => f.Name == rf.Name)) // ... available via calculation
                        || h.ManifestsBySubsumption!.Any(mfm => mfm.Features!.Any(f => f.Name == rf.Name)))))) // ... or available via subsumption
            .Select(rm => new ModWithMissingDependencyMod
            (
                rm.ModFileManfiestId,
                rm.RequirementIdentifier == null ? null : rm.RequirementIdentifier.Identifier,
                rm.ModFileManifest!.RequiredMods!.Count(orm => orm.RequirementIdentifier == rm.RequirementIdentifier),
                rm.ModFileManifest!.Name,
                rm.ModFileManifest!.Creators!.Select(c => c.Name).ToList(),
                rm.ModFileManifest!.Url,
                rm.Name,
                rm.Creators!.Select(c => c.Name).ToList(),
                rm.Url,
                rm.ModFileManifest!.ModFileHash!.ModFiles!.Where(mf => mf.Path != null && mf.AbsenceNoticed == null).Select(mf => mf.Path!).ToList(),
                rm.Hashes!.All(h => // all hashes are
                    h.ManifestsByCalculation!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null)) //... available via calculation
                        || h.ManifestsBySubsumption!.Any(mfm => mfm.ModFileHash!.ModFiles!.Any(mf => mf.Path != null && mf.AbsenceNoticed == null))) // or available via subsumption
            ))
            .AsAsyncEnumerable())
        {
            if (modWithMissingDependencyMod.RequirementIdentifier is null)
            {
                modsWithMissingDependencyMod.Add(modWithMissingDependencyMod);
                continue;
            }
            var key = (modWithMissingDependencyMod.ModManifestId, modWithMissingDependencyMod.RequirementIdentifier);
            if (!commonRequirementIdentifiers.TryGetValue(key, out var list))
            {
                list = [];
                commonRequirementIdentifiers.Add(key, list);
            }
            list.Add(modWithMissingDependencyMod);
        }
        static string getByLine(IReadOnlyList<string> creators) =>
            creators.Any() ? $" by {creators.Humanize()}" : string.Empty;
        static (string caption, string description, ScanIssueResolution? resolution) getResolution(ModWithMissingDependencyMod modWithMissingDependencyMod)
        {
            var resolutionType = MissingDependencyModResolutionType.Normal;
            if (string.IsNullOrWhiteSpace(modWithMissingDependencyMod.Name))
                resolutionType |= MissingDependencyModResolutionType.UnnamedDependent;
            if (modWithMissingDependencyMod.Url is null)
                resolutionType |= MissingDependencyModResolutionType.UnspecifiedDependentSource;
            if (string.IsNullOrWhiteSpace(modWithMissingDependencyMod.DependencyName))
                resolutionType |= MissingDependencyModResolutionType.UnnamedDependency;
            if (modWithMissingDependencyMod.DependencyUrl is null)
                resolutionType |= MissingDependencyModResolutionType.UnspecifiedDependencySource;
            if (modWithMissingDependencyMod.Name == modWithMissingDependencyMod.DependencyName)
                resolutionType |= MissingDependencyModResolutionType.IdenticallyNamed;
            return resolutionType switch
            {
                MissingDependencyModResolutionType.BrokenFile =>
                (
                    $"A Mod Needs Another Mod Installed",
                    $"""
                    I've found {"file".ToQuantity(modWithMissingDependencyMod.FilePaths.Count, ShowQuantityAs.Words)} in your Mods folder ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}) which require{(modWithMissingDependencyMod.FilePaths.Count is 1 ? "s" : string.Empty)} that you also have another mod installed... and I can't find it. Unfortunately, I don't even know its name and have no idea where to even send you to download a fresh copy of either. 😱
                    """,
                    null
                ),
                MissingDependencyModResolutionType.ReinstallFile =>
                (
                    $"A Mod Needs Another Mod Installed",
                    $"""
                    I've found {"file".ToQuantity(modWithMissingDependencyMod.FilePaths.Count, ShowQuantityAs.Words)} in your Mods folder ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}) which require{(modWithMissingDependencyMod.FilePaths.Count is 1 ? "s" : string.Empty)} that you also have another mod installed... and I can't find it. Unfortunately, I don't even know its name, but I *do know* you to re-download the original mod before this gets bad. 😨
                    """,
                    new()
                    {
                        Label = $"Go Re-download {modWithMissingDependencyMod.Name}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependent",
                        Url = modWithMissingDependencyMod.Url
                    }
                ),
                MissingDependencyModResolutionType.ReinstallMod =>
                (
                    $"{modWithMissingDependencyMod.Name} Needs Another Mod Installed",
                    $"""
                    You have **{modWithMissingDependencyMod.Name}**{getByLine(modWithMissingDependencyMod.Creators)} installed ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}), which requires that you also have another mod installed... and I can't find it. Unfortunately, I don't even know its name, but I *do know* **{modWithMissingDependencyMod.Name}** needs you to return to its web site to see if you can find this other mod it's missing. 😓
                    """,
                    new()
                    {
                        Label = $"Go Re-download {modWithMissingDependencyMod.Name}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependent",
                        Url = modWithMissingDependencyMod.Url
                    }
                ),
                MissingDependencyModResolutionType.FileNeedsDownload =>
                (
                    "A Mod Needs Another Mod Installed",
                    $"""
                    I've found {"file".ToQuantity(modWithMissingDependencyMod.FilePaths.Count, ShowQuantityAs.Words)} in your Mods folder ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}) which require{(modWithMissingDependencyMod.FilePaths.Count is 1 ? "s" : string.Empty)} that you also have another mod installed. I'm sorry that I can't tell you what either of these mods is called, but I can assure you that this is a problem. 😭
                    """,
                    new()
                    {
                        Label = $"Go Download the Needed Mod",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependency",
                        Url = modWithMissingDependencyMod.DependencyUrl
                    }
                ),
                MissingDependencyModResolutionType.ModNeedsDownload =>
                (
                    $"{modWithMissingDependencyMod.Name} Needs Another Mod Installed",
                    $"""
                    You have **{modWithMissingDependencyMod.Name}**{getByLine(modWithMissingDependencyMod.Creators)} installed ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}), which requires that you also have another mod installed. I'm sorry that I can't tell you what that other mod is called but I can assure you of one thing... I can't find it. 🤷
                    """,
                    new()
                    {
                        Label = $"Go Download the Other Mod",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependency",
                        Url = modWithMissingDependencyMod.DependencyUrl
                    }
                ),
                MissingDependencyModResolutionType.FileNeedsDependency =>
                (
                    $"Some Mods Need {modWithMissingDependencyMod.DependencyName} Installed",
                    $"""
                    I've found {"file".ToQuantity(modWithMissingDependencyMod.FilePaths.Count, ShowQuantityAs.Words)} in your Mods folder ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}) which require{(modWithMissingDependencyMod.FilePaths.Count is 1 ? "s" : string.Empty)} that you also have **{modWithMissingDependencyMod.DependencyName}**{getByLine(modWithMissingDependencyMod.DependencyCreators)} installed, and unfortunately... I can't find it. 🤷
                    """,
                    new()
                    {
                        Label = $"Go Download {modWithMissingDependencyMod.DependencyName}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependency",
                        Url = modWithMissingDependencyMod.DependencyUrl
                    }
                ),
                MissingDependencyModResolutionType type when type.HasFlag(MissingDependencyModResolutionType.IdenticallyNamed) =>
                (
                    $"{modWithMissingDependencyMod.Name} is Missing a File",
                    $"""
                    You have **{modWithMissingDependencyMod.Name}**{getByLine(modWithMissingDependencyMod.Creators)} installed ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}), and it seems one of its files specified as necessary is not in your Mods folder. 🤷
                    """,
                    new()
                    {
                        Label = $"Go Re-download {modWithMissingDependencyMod.Name}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadComponent",
                        Url = modWithMissingDependencyMod.Url
                    }
                ),
                _ =>
                (
                    $"{modWithMissingDependencyMod.Name} Needs {modWithMissingDependencyMod.DependencyName} Installed",
                    $"""
                    You have **{modWithMissingDependencyMod.Name}**{getByLine(modWithMissingDependencyMod.Creators)} installed ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}), which requires that you also have **{modWithMissingDependencyMod.DependencyName}**{getByLine(modWithMissingDependencyMod.DependencyCreators)} installed, and unfortunately... I can't find it. 🤷
                    """,
                    new()
                    {
                        Label = $"Go Download {modWithMissingDependencyMod.DependencyName}",
                        Icon = MaterialDesignIcons.Normal.OpenInNew,
                        Color = MudBlazor.Color.Primary,
                        Data = "downloadDependency",
                        Url = modWithMissingDependencyMod.DependencyUrl
                    }
                )
            };
        }
        var commonRequirementIdentifiersValuesBySolitude = commonRequirementIdentifiers.Values
            .Where(list => list.Count == list.First().CommonRequirementIdentifiers)
            .ToLookup(list => list.Count is 1);
        foreach (var modWithMissingDependencyMod in modsWithMissingDependencyMod.Concat(commonRequirementIdentifiersValuesBySolitude[true].SelectMany(list => list)))
        {
            var (caption, description, resolution) = getResolution(modWithMissingDependencyMod);
            yield return new()
            {
                Caption = caption,
                Description = description,
                Icon = MaterialDesignIcons.Outline.PuzzleRemove,
                Type = ScanIssueType.Sick,
                Origin = this,
                Data = modWithMissingDependencyMod,
                Resolutions =
                    [..(resolution is null
                        ? Enumerable.Empty<ScanIssueResolution>()
                        : [resolution])
                    .Concat
                    (
                        modWithMissingDependencyMod.FilePaths.Select(filePath => new ScanIssueResolution()
                        {
                            Label = $"Show me the file for {modWithMissingDependencyMod.Name ?? "the mod"}",
                            Icon = MaterialDesignIcons.Normal.OpenInNew,
                            Color = MudBlazor.Color.Default,
                            Data = $"showfile-{filePath}"
                        })
                    )]
            };
        }
        foreach (var list in commonRequirementIdentifiersValuesBySolitude[false])
        {
            var modWithMissingDependencyMod = list.First();
            yield return new()
            {
                Caption = $"{modWithMissingDependencyMod.Name} has an Unmet Requirement",
                Description =
                    $"""
                    You have **{modWithMissingDependencyMod.Name}**{getByLine(modWithMissingDependencyMod.Creators)} installed ({modWithMissingDependencyMod.FilePaths.Select(filePath => $"`{filePath}`").Humanize()}) which has an unmet requirement which it calls "{modWithMissingDependencyMod.RequirementIdentifier}". But, you know, it's kinda chill. It says you can actually meet the requirement in more than one way. What do you want to do? 🤷
                    """,
                Icon = MaterialDesignIcons.Outline.PuzzleRemove,
                Type = ScanIssueType.Sick,
                Origin = this,
                Data = list,
                Resolutions =
                    [..list.Select(modWithMissingDependencyMod => getResolution(modWithMissingDependencyMod).resolution)
                    .Concat
                    (
                        modWithMissingDependencyMod.FilePaths.Select(filePath => new ScanIssueResolution()
                        {
                            Label = $"Show me the file for {modWithMissingDependencyMod.Name ?? "the mod"}",
                            Icon = MaterialDesignIcons.Normal.OpenInNew,
                            Color = MudBlazor.Color.Default,
                            Data = $"showfile-{filePath}"
                        })
                    )]
            };
        }
    }
}
