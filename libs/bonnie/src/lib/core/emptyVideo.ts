import { Video } from "../generated";


export function emptyVideo(): Video {
  return {
    $type: 'video', // todo: remove
    type: 'video',
    sources: [],
  };
}
