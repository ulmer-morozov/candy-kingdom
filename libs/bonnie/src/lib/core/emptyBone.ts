import { Bone } from "../generated";


export function emptyBone(): Bone {
  return {
    style: '',
    mediaQuery: '',
    enabled: true,
    type: ''
  };
}
