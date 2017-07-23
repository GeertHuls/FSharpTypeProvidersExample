
-- CREATE NEW DATABASE: 'MovieNews'

CREATE TABLE [dbo].[LatestMovies] (
	[Title]				NVARCHAR(100)	NOT NULL,
	[Description]		NVARCHAR(1000)	NOT NULL,
	[Thumbnail]			NVARCHAR(200)	NULL,
	[SortOrder]			INT				NOT NULL
)
