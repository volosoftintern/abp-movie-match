
export interface CreateMovieDto {
  id: number;
  title?: string;
  posterPath?: string;
  overview?: string;
}

export interface GenreDto {
  id: number;
  name?: string;
}

export interface MovieDetailDto {
  id: number;
  title?: string;
  imdbId?: string;
  overview?: string;
  posterPath?: string;
  voteAverage: number;
  releaseDate?: string;
  director: PersonDto;
  stars: PersonDto[];
  genres: GenreDto[];
  isActiveWatchLater: boolean;
  isActiveWatchedBefore: boolean;
}

export interface MovieDto {
  id: number;
  title?: string;
  posterPath?: string;
  overview?: string;
  isActiveWatchLater: boolean;
  isActiveWatchedBefore: boolean;
}

export interface PersonDto {
  id: number;
  name?: string;
  profilePath?: string;
  birthDay?: string;
  deathDay?: string;
  biography?: string;
  movies: MovieDto[];
}
