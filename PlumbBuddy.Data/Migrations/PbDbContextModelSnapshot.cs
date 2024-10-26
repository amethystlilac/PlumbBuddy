﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlumbBuddy.Data;

#nullable disable

namespace PlumbBuddy.Data.Migrations
{
    [DbContext(typeof(PbDbContext))]
    partial class PbDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("ModCreatorModFileManifest", b =>
                {
                    b.Property<long>("AttributedModsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CreatorsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AttributedModsId", "CreatorsId");

                    b.HasIndex("CreatorsId");

                    b.ToTable("ModCreatorModFileManifest");
                });

            modelBuilder.Entity("ModCreatorRequiredMod", b =>
                {
                    b.Property<long>("AttributedRequiredModsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CreatorsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AttributedRequiredModsId", "CreatorsId");

                    b.HasIndex("CreatorsId");

                    b.ToTable("ModCreatorRequiredMod");
                });

            modelBuilder.Entity("ModExclusivityModFileManifest", b =>
                {
                    b.Property<long>("ExclusivitiesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SpecifiedByModFileManifestsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ExclusivitiesId", "SpecifiedByModFileManifestsId");

                    b.HasIndex("SpecifiedByModFileManifestsId");

                    b.ToTable("ModExclusivityModFileManifest");
                });

            modelBuilder.Entity("ModFeatureModFileManifest", b =>
                {
                    b.Property<long>("FeaturesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SpecifiedByModFileManifestsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FeaturesId", "SpecifiedByModFileManifestsId");

                    b.HasIndex("SpecifiedByModFileManifestsId");

                    b.ToTable("ModFeatureModFileManifest");
                });

            modelBuilder.Entity("ModFeatureRequiredMod", b =>
                {
                    b.Property<long>("RequiredFeaturesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SpecifiedByRequiredModsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RequiredFeaturesId", "SpecifiedByRequiredModsId");

                    b.HasIndex("SpecifiedByRequiredModsId");

                    b.ToTable("ModFeatureRequiredMod");
                });

            modelBuilder.Entity("ModFileManifestHashRequiredMod", b =>
                {
                    b.Property<long>("DependentsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("HashesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DependentsId", "HashesId");

                    b.HasIndex("HashesId");

                    b.ToTable("ModFileManifestHashRequiredMod");
                });

            modelBuilder.Entity("ModFileManifestModFileManifestHash", b =>
                {
                    b.Property<long>("ManifestsBySubsumptionId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SubsumedHashesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ManifestsBySubsumptionId", "SubsumedHashesId");

                    b.HasIndex("SubsumedHashesId");

                    b.ToTable("ModFileManifestModFileManifestHash");
                });

            modelBuilder.Entity("ModFileManifestPackCode", b =>
                {
                    b.Property<long>("IncompatiblePacksId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("IncompatibleWithModsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IncompatiblePacksId", "IncompatibleWithModsId");

                    b.HasIndex("IncompatibleWithModsId");

                    b.ToTable("ModFileManifestPackCode");
                });

            modelBuilder.Entity("ModFileManifestPackCode1", b =>
                {
                    b.Property<long>("RequiredByModsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("RequiredPacksId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RequiredByModsId", "RequiredPacksId");

                    b.HasIndex("RequiredPacksId");

                    b.ToTable("ModFileManifestPackCode1");
                });

            modelBuilder.Entity("ModFileResourceTopologySnapshot", b =>
                {
                    b.Property<long>("ResourcesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("TopologySnapshotsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ResourcesId", "TopologySnapshotsId");

                    b.HasIndex("TopologySnapshotsId");

                    b.ToTable("ModFileResourceTopologySnapshot");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ElectronicArtsPromoCode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ElectronicArtsPromoCodes");
                });

            modelBuilder.Entity("PlumbBuddy.Data.FileOfInterest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Path")
                        .IsUnique();

                    b.ToTable("FilesOfInterest");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModCreator", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ModCreators");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModExclusivity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ModExclusivities");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFeature", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ModFeatures");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("AbsenceNoticed")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("Creation")
                        .HasColumnType("TEXT");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LastWrite")
                        .HasColumnType("TEXT");

                    b.Property<long>("ModFileHashId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModFileHashId");

                    b.HasIndex("Path")
                        .IsUnique();

                    b.HasIndex("Path", "Creation", "LastWrite", "Size");

                    b.ToTable("ModFiles");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileHash", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ResourcesAndManifestsCataloged")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Sha256")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("BLOB")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("Sha256")
                        .IsUnique();

                    b.ToTable("ModFileHashes");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CalculatedModFileManifestHashId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ElectronicArtsPromoCodeId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("InscribedModFileManifestHashId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("KeyFullInstance")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("KeyGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("KeyType")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModFileHashId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("TuningFullInstance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TuningName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CalculatedModFileManifestHashId");

                    b.HasIndex("ElectronicArtsPromoCodeId");

                    b.HasIndex("InscribedModFileManifestHashId");

                    b.HasIndex("ModFileHashId");

                    b.ToTable("ModFileManifests");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifestHash", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Sha256")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("BLOB")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("Sha256")
                        .IsUnique();

                    b.ToTable("ModFileManifestHashes");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifestResourceKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("KeyFullInstance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KeyGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KeyType")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModFileManifestId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModFileManifestId");

                    b.ToTable("ModFileManifestResourceKeys");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileResource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("KeyFullInstance")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KeyGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KeyType")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModFileHashId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModFileHashId");

                    b.ToTable("ModFileResources");
                });

            modelBuilder.Entity("PlumbBuddy.Data.PackCode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("PackCodes");
                });

            modelBuilder.Entity("PlumbBuddy.Data.RequiredMod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("IgnoreIfHashAvailableId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("IgnoreIfHashUnavailableId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("IgnoreIfPackAvailableId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("IgnoreIfPackUnavailableId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModFileManfiestId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("RequirementIdentifierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IgnoreIfHashAvailableId");

                    b.HasIndex("IgnoreIfHashUnavailableId");

                    b.HasIndex("IgnoreIfPackAvailableId");

                    b.HasIndex("IgnoreIfPackUnavailableId");

                    b.HasIndex("ModFileManfiestId");

                    b.HasIndex("RequirementIdentifierId");

                    b.ToTable("RequiredMods");
                });

            modelBuilder.Entity("PlumbBuddy.Data.RequirementIdentifier", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Identifier")
                        .IsUnique();

                    b.ToTable("RequirementIdentifiers");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ScriptModArchiveEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<long>("CompressedLength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExternalAttributes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEncrypted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LastWriteTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ModFileHashId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SignedCrc32")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModFileHashId", "FullName")
                        .IsUnique();

                    b.ToTable("ScriptModArchiveEntries");
                });

            modelBuilder.Entity("PlumbBuddy.Data.TopologySnapshot", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Taken")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TopologySnapshots");
                });

            modelBuilder.Entity("ModCreatorModFileManifest", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("AttributedModsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModCreator", null)
                        .WithMany()
                        .HasForeignKey("CreatorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModCreatorRequiredMod", b =>
                {
                    b.HasOne("PlumbBuddy.Data.RequiredMod", null)
                        .WithMany()
                        .HasForeignKey("AttributedRequiredModsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModCreator", null)
                        .WithMany()
                        .HasForeignKey("CreatorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModExclusivityModFileManifest", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModExclusivity", null)
                        .WithMany()
                        .HasForeignKey("ExclusivitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("SpecifiedByModFileManifestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFeatureModFileManifest", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFeature", null)
                        .WithMany()
                        .HasForeignKey("FeaturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("SpecifiedByModFileManifestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFeatureRequiredMod", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFeature", null)
                        .WithMany()
                        .HasForeignKey("RequiredFeaturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.RequiredMod", null)
                        .WithMany()
                        .HasForeignKey("SpecifiedByRequiredModsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFileManifestHashRequiredMod", b =>
                {
                    b.HasOne("PlumbBuddy.Data.RequiredMod", null)
                        .WithMany()
                        .HasForeignKey("DependentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", null)
                        .WithMany()
                        .HasForeignKey("HashesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFileManifestModFileManifestHash", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("ManifestsBySubsumptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", null)
                        .WithMany()
                        .HasForeignKey("SubsumedHashesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFileManifestPackCode", b =>
                {
                    b.HasOne("PlumbBuddy.Data.PackCode", null)
                        .WithMany()
                        .HasForeignKey("IncompatiblePacksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("IncompatibleWithModsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFileManifestPackCode1", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifest", null)
                        .WithMany()
                        .HasForeignKey("RequiredByModsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.PackCode", null)
                        .WithMany()
                        .HasForeignKey("RequiredPacksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ModFileResourceTopologySnapshot", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileResource", null)
                        .WithMany()
                        .HasForeignKey("ResourcesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.TopologySnapshot", null)
                        .WithMany()
                        .HasForeignKey("TopologySnapshotsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFile", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileHash", "ModFileHash")
                        .WithMany("ModFiles")
                        .HasForeignKey("ModFileHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModFileHash");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifest", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", "CalculatedModFileManifestHash")
                        .WithMany("ManifestsByCalculation")
                        .HasForeignKey("CalculatedModFileManifestHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ElectronicArtsPromoCode", "ElectronicArtsPromoCode")
                        .WithMany("ReferencingModFileManifests")
                        .HasForeignKey("ElectronicArtsPromoCodeId");

                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", "InscribedModFileManifestHash")
                        .WithMany("ManifestsByInscription")
                        .HasForeignKey("InscribedModFileManifestHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.ModFileHash", "ModFileHash")
                        .WithMany("ModFileManifests")
                        .HasForeignKey("ModFileHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CalculatedModFileManifestHash");

                    b.Navigation("ElectronicArtsPromoCode");

                    b.Navigation("InscribedModFileManifestHash");

                    b.Navigation("ModFileHash");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifestResourceKey", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifest", "ModFileManifest")
                        .WithMany("HashResourceKeys")
                        .HasForeignKey("ModFileManifestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModFileManifest");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileResource", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileHash", "ModFileHash")
                        .WithMany("Resources")
                        .HasForeignKey("ModFileHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModFileHash");
                });

            modelBuilder.Entity("PlumbBuddy.Data.RequiredMod", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", "IgnoreIfHashAvailable")
                        .WithMany("DisqualifyingByPresence")
                        .HasForeignKey("IgnoreIfHashAvailableId");

                    b.HasOne("PlumbBuddy.Data.ModFileManifestHash", "IgnoreIfHashUnavailable")
                        .WithMany("DisqualifyingByAbsence")
                        .HasForeignKey("IgnoreIfHashUnavailableId");

                    b.HasOne("PlumbBuddy.Data.PackCode", "IgnoreIfPackAvailable")
                        .WithMany("DisqualifyingByPresence")
                        .HasForeignKey("IgnoreIfPackAvailableId");

                    b.HasOne("PlumbBuddy.Data.PackCode", "IgnoreIfPackUnavailable")
                        .WithMany("DisqualifyingByAbsence")
                        .HasForeignKey("IgnoreIfPackUnavailableId");

                    b.HasOne("PlumbBuddy.Data.ModFileManifest", "ModFileManifest")
                        .WithMany("RequiredMods")
                        .HasForeignKey("ModFileManfiestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlumbBuddy.Data.RequirementIdentifier", "RequirementIdentifier")
                        .WithMany("RequirementGroupMembers")
                        .HasForeignKey("RequirementIdentifierId");

                    b.Navigation("IgnoreIfHashAvailable");

                    b.Navigation("IgnoreIfHashUnavailable");

                    b.Navigation("IgnoreIfPackAvailable");

                    b.Navigation("IgnoreIfPackUnavailable");

                    b.Navigation("ModFileManifest");

                    b.Navigation("RequirementIdentifier");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ScriptModArchiveEntry", b =>
                {
                    b.HasOne("PlumbBuddy.Data.ModFileHash", "ModFileHash")
                        .WithMany("ScriptModArchiveEntries")
                        .HasForeignKey("ModFileHashId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModFileHash");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ElectronicArtsPromoCode", b =>
                {
                    b.Navigation("ReferencingModFileManifests");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileHash", b =>
                {
                    b.Navigation("ModFileManifests");

                    b.Navigation("ModFiles");

                    b.Navigation("Resources");

                    b.Navigation("ScriptModArchiveEntries");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifest", b =>
                {
                    b.Navigation("HashResourceKeys");

                    b.Navigation("RequiredMods");
                });

            modelBuilder.Entity("PlumbBuddy.Data.ModFileManifestHash", b =>
                {
                    b.Navigation("DisqualifyingByAbsence");

                    b.Navigation("DisqualifyingByPresence");

                    b.Navigation("ManifestsByCalculation");

                    b.Navigation("ManifestsByInscription");
                });

            modelBuilder.Entity("PlumbBuddy.Data.PackCode", b =>
                {
                    b.Navigation("DisqualifyingByAbsence");

                    b.Navigation("DisqualifyingByPresence");
                });

            modelBuilder.Entity("PlumbBuddy.Data.RequirementIdentifier", b =>
                {
                    b.Navigation("RequirementGroupMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
